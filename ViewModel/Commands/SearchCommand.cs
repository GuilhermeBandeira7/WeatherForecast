using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WeatherApp.ViewModel.Commands
{
    public class SearchCommand : ICommand //Responsible Executing a command of a button in the UI
    {
        public WeatherVM VM { get; set; }

        //Implementation of ICommand interface
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value;}
        }

        public SearchCommand(WeatherVM vM)
        {
            VM = vM;    
        }

        /// <summary>
        /// Evaluate and return true if the there are content to be queried in the TextBlock, Else return false.
        /// </summary>
        /// <param name="parameter">Content of the textblock with the name of the city</param>
        /// <returns></returns>
        public bool CanExecute(object? parameter)
        {
            string? query = parameter as string;
            if (string.IsNullOrWhiteSpace(query))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// If the operation is allowed to execute, make query request to get a List of cities
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object? parameter)
        {
            VM.MakeQuery();
        }
    }
}
