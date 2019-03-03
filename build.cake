///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var sln= new FilePath("EifelMono.Fluent.sln");



Task("Clean")
.Does(()=> {
    for (int i= 0;i< 5; i++) // Try it more times.....
    try {
        CleanDirectories($"./src/**/bin/{configuration}/**/**");
        return;
    }
    catch {}
});

Task("Restore")
.Does(() => {
    DotNetCoreRestore(sln.FullPath);
});

Task("Build")
.Does(() => {
    DotNetCoreBuild(sln.FullPath, new DotNetCoreBuildSettings{
        Configuration= configuration
    });
});

Task("CleanNuget")
.Does(()=> {
     CleanDirectories($"./nuget");
});

Task("CopyNuget")
.Does(()=> {
    foreach(var file in GetFiles($"./src/**/bin/{configuration}/*.nupkg"))
        CopyFile(file, $"./nuget/{file.GetFilename()}");
});

Task("Doc")
.Does(()=> {
        StartAndReturnProcess("generateomd", new ProcessSettings
            { Arguments = $"/source=./src" }
        );
});


Task("Default")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Build")
.IsDependentOn("CleanNuget")
.IsDependentOn("CopyNuget")
.Does(() => {
});




Task("DirTest")
.Does(() => {
    var files= GetFiles("./**/*.Test/*.cs");
    foreach(var file in files)
        Information(file);
    Information($"count={files.Count()}");
});


Task("DirTestB")
.Does(() => {
    var files= GetFiles("./src/**/*.cs");
    foreach(var file in files)
        Information(file);
    Information($"count={files.Count()}");
});

Task("DirTestC")
.Does(() => {
    var files= GetFiles("./src/**/*.Fluent.*/**/*.cs");
    foreach(var file in files)
        Information(file);
    Information($"count={files.Count()}");
});

Task("DirTestD")
.Does(() => {
    var files= GetFiles("./**/*.cs");
    foreach(var file in files)
        Information(file);
    Information($"count={files.Count()}");
    files= GetFiles("./**/**/*.cs");
    foreach(var file in files)
        Information(file);
    Information($"count={files.Count()}");
});

Task("DirTestE")
.Does(() => {
    var files= GetFiles("./**/**.cs");
    foreach(var file in files)
        Information(file);
    Information($"count={files.Count()}");
    files= GetFiles("");
    foreach(var file in files)
        Information(file);
    Information($"count={files.Count()}");
});

void ListDirs(string mask, bool output= false)
{
    Information(mask);
    var directories= GetDirectories(mask);
    if (output)
    {
        foreach(var directory in directories)
            Information(directory);
    }
    Information($"count={directories.Count()}");
}

Task("DirTestF")
.Does(() => {
    // 38
    // ListDirs("./src/**", false);
    // 37
    // ListDirs("./src/**/*/**", true);
    // 19
    //ListDirs("./src/**/EifelMono.Fluent/**", true);
    // 6
    // ListDirs("./src/**/EifelMono.Fluent/*", true);
    // 4
    // ListDirs("./src/**/EifelMono.Fluent.*/*", true);
    // 11
    // ListDirs("./src/**/*Test/**");
    ListDirs("./src/**/net*");
});

void ListFiles(string mask, bool output= false)
{
    Information(mask);
    var files= GetFiles(mask);
    if (output)
    {
        foreach(var file in files)
            Information(file);
    }
    Information($"count={files.Count()}");
}

Task("FileTestA")
.Does(() => {
    ListFiles("./src/**/*.cs", true);
    ListFiles("./src/**/*.dll", true);
});



RunTarget(target);
