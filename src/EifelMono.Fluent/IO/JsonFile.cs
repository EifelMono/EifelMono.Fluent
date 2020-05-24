using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Log;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace EifelMono.Fluent.IO
{
    namespace ProLog.Core.IO
    {
        public class JsonFile
        {
            public static readonly JsonSerializerSettings s_jsonSerializerSettings
                = new JsonSerializerSettings { Formatting = Formatting.Indented }
                    .Pipe(settings => settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()));

            public static T JsonToObject<T>(string json) where T : class
                => JsonConvert.DeserializeObject<T>(json, s_jsonSerializerSettings);
            public static string ObjectToJson(object @object)
                => JsonConvert.SerializeObject(@object, s_jsonSerializerSettings);

            public static (bool Ok, T Value) JsonToObjectSafe<T>(string json) where T : class, new()
            {
                try
                {
                    return (true, JsonToObject<T>(json));
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                return (false, new T());
            }

            public static (bool Ok, string Value) ObjectToJsonSafe(object @object)
            {
                try
                {
                    return (true, ObjectToJson(@object));
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                return (false, "");
            }

            public FilePath FilePath { get; set; }
            public FilePath BackupFilePath { get; set; }
            public JsonFile(FilePath filePath)
            {
                FilePath = filePath;
                BackupFilePath = FilePath
                    .Clone()
                    .AppendDirectoryName($".{FilePath.FileName}")
                    .ChangeFileNameWithoutExtensionAppend(FilePath.DateTimeFormat.yyyyMMddHHmmss);
            }
        }

        public class JsonFile<T> : JsonFile where T : class, new()
        {
            public Type DataType { get; set; } = typeof(T);

            private readonly Func<JsonFile<T>, T, string> _onWriteConvertToJson;
            public JsonFile(FilePath filePath, Func<JsonFile<T>, T, string> onWriteConvertToJson = null, bool writeSchema = true) : base(filePath)
            {
                _onWriteConvertToJson = onWriteConvertToJson;
                if (writeSchema)
                    WriteSchema();
            }

            public (bool Ok, T Data, bool FirstInstallation) Read()
            {
                if (FilePath.Exists)
                {
                    var data = new T();
                    try
                    {
                        data = JsonToObject<T>(FilePath.ReadAllText());
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        return (false, data, false);
                    }
                    return (true, data, false);
                }
                else
                {
                    // First installation !
                    if (!BackupFilePath.Directory.Exists)
                    {
                        return (true, new T(), true);
                    }
                }
                return (false, new T(), false);
            }

            public bool Write(T data, CancellationToken stoppingToken = default, bool withCheck = true)
            {
                _ = WriteBackupAsync(data, stoppingToken);
                var tries = 0;
                var maxTries = 10;
                var ok = false;
                while (!ok)
                    try
                    {
                        var json = "";
                        if (_onWriteConvertToJson is object)
                            json = _onWriteConvertToJson(this, data);
                        else
                            json = ObjectToJson(data);
                        FilePath.WriteAllText(json);
                        ok = true;
                        if (withCheck)
                            _ = JsonToObject<T>(FilePath.ReadAllText());
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        tries++;
                        if (tries > maxTries)
                        {
                            ex.LogException($"Write {FilePath} to disk failed");
                            return false;
                        }
                        if (stoppingToken.IsCancellationRequested)
                            break;
                    }
                return true;
            }

            public async Task WriteBackupAsync(T data, CancellationToken stoppingToken = default)
            {
                try
                {
                    var json = "";
                    if (_onWriteConvertToJson is object)
                        json = _onWriteConvertToJson(this, data);
                    else
                        json = ObjectToJson(data);
                    BackupFilePath.EnsureDirectoryExist()
                        .WriteAllText(json);
                }
                catch (Exception ex)
                {
                    ex.LogException($"Write {BackupFilePath} to disk failed ");
                }
                await CleanBackupAsync(stoppingToken);
            }

            private DateTime _lastCleanBackAsync = DateTime.Now.AddDays(-1);
            public async Task CleanBackupAsync(CancellationToken stoppingToken)
            {
                if ((DateTime.Now - _lastCleanBackAsync).Days > 0)
                {
                    _lastCleanBackAsync = DateTime.Now;
                    await Task.Run(() =>
                    {
                        BackupFilePath.Directory.GetFiles($"*{FilePath.Extension}")
                           .Select(f => new
                           {
                               File = f,
                               Age = AgeInDaysFromFileName(f)
                           }).Where(nf => nf.Age > 30).ForEach(nf =>
                           {
                               try
                               {
                                   nf.File.Delete();
                               }
                               catch { }
                           });
                    }, stoppingToken);
                }
            }

            public DateTime TimeStampFromFileName(FilePath filePath)
            {
                var split = filePath.FileName.Split('.');
                if (split.Length > 1)
                    if (DateTime.TryParseExact(split[split.Length - 2],
                        "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                        return dateTime;
                return DateTime.MaxValue;
            }

            public int AgeInDaysFromFileName(FilePath filePath)
                => (DateTime.Now - TimeStampFromFileName(filePath)).Days;

            public bool IsTextRegularJson(string text)
            {
                var resultObject = JsonToObjectSafe<T>(text);
                if (!resultObject.Ok)
                    return false;
                var (Ok, _) = ObjectToJsonSafe(resultObject.Value);
                if (!Ok)
                    return false;
                return true;
            }

            public TNew ConvertToObject<TNew>(T data) where TNew : class
                => JsonToObject<TNew>(ObjectToJson(data));

            public string ConvertToJson<TNew>(T data) where TNew : class
                => ObjectToJson(ConvertToObject<TNew>(data));

            public (bool Ok, string Value, FilePath filePath) ReadAllTextSafe(string defaultValue = "")
                => FilePath.ReadAllTextSafe(defaultValue);

            public bool WriteAllText(string text)
            {
                try
                {
                    FilePath.WriteAllText(text);
                    return true;
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                return false;
            }

            public JSchema GenerateSchema()
            {
                try
                {
                    var jsonSchemaGenerator = new JSchemaGenerator();
                    jsonSchemaGenerator.GenerationProviders.Add(new StringEnumGenerationProvider());
                    var type = typeof(T);
                    var schema = jsonSchemaGenerator.Generate(type);
                    schema.Title = type.Name;
                    return schema;
                }
                catch { }
                return new JSchema();
            }

            public void WriteSchema()
            {
                try
                {
                    var ext = FilePath.Extension;
                    FilePath.Clone().ChangeExtension($".schema{ext}").WriteAllText(GenerateSchema().ToString());
                }
                catch { }
            }
        }
    }
}
