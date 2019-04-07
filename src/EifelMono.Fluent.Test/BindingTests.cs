using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EifelMono.Fluent.Bindings;
using EifelMono.Fluent.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test
{
    public class BindingCommandTests : XunitCore
    {
        public BindingCommandTests(ITestOutputHelper output) : base(output) { }

        public class TestClass : INotifyPropertyChanged, IOnPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            int _command1Count = 0;
            public int Command1Count
            {
                get => this.PropertyGet(ref _command1Count);
                set => this.PropertySet(ref _command1Count, value);
            }

            ICommand _command1 = null;
            public ICommand Command1 => this.Command(ref _command1, () =>
            {
                Command1Count++;
            });

            string _command2Count = "0";
            public string Command2Count
            {
                get => this.PropertyGet(ref _command2Count);
                set => this.PropertySet(ref _command2Count, value);
            }

            object _command2Parameter;
            public object Command2Parameter
            {
                get => this.PropertyGet(ref _command2Parameter);
                set => this.PropertySet(ref _command2Parameter, value);
            }

            ICommand _command2 = null;
            public ICommand Command2 => this.Command(ref _command2, (parameter) =>
            {
                Command2Parameter = parameter;
                Command2Count = (Command2Count.ToInt() + 1).ToString();
            });

            public BindingCollection<string> Collection1 { get; set; } = new BindingCollection<string>();
        }


        [Fact]
        public void CreateCommand_AndTestCreationCall()
        {
            {
                ICommand _command = null;
                var command = BindingCommand.Create(ref _command, () =>
                {

                });
                var safeCommand = command;
                var nextCommand = command;
                Assert.Equal(safeCommand, nextCommand);
            }
            {
                ICommand _command = null;
                var command = BindingCommand.Create(ref _command, (parameter) =>
                {

                });
                var safeCommand = command;
                var nextCommand = command;
                Assert.Equal(safeCommand, nextCommand);
            }
        }

        [Fact]
        public void TestClass_Tets()
        {
            var testClass = new TestClass
            {
                Command1Count = 0,
                Command2Count = "0",
                Collection1 = new BindingCollection<string>
                {
                    "1",
                    "2"
                }
            };

            Assert.Equal(2, testClass.Collection1.Count);
            testClass.Collection1
                .AddItems(new List<string> { "3", "4" })
                .AddItems(new List<string> { "5", "6" });
            Assert.Equal(6, testClass.Collection1.Count);
            testClass.Command1.Execute("");
            Assert.True(testClass.Command1.CanExecute(""));
            Assert.Equal(1, testClass.Command1Count);
            testClass.Command2.Execute("");
            Assert.Equal("1", testClass.Command2Count);


        }
    }
}
