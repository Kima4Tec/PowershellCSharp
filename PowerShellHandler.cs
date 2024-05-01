using System.Management.Automation;
using System.Text;

namespace PowershellCSharp
{
    public static class PowerShellHandler
    {
        private static PowerShell ps = PowerShell.Create();

        public static string Command(string script)
        {
            string errorMsg = string.Empty;
            string output = string.Empty;

            ps.AddScript(script);

            //Make sure return values are outputted to the stream by C#
            ps.AddCommand("Out-String");

            PSDataCollection<PSObject> outputCollection = new();
            ps.Streams.Error.DataAdded += (object sender, DataAddedEventArgs e) =>
            {
                errorMsg = ((PSDataCollection<ErrorRecord>)sender)[e.Index].ToString();
            };

            IAsyncResult result = ps.BeginInvoke<PSObject, PSObject>(null, outputCollection);

            //Wait for powershell command/script to finsh executing
            ps.EndInvoke(result);

            StringBuilder sb = new();

            foreach (var outputItem in outputCollection) 
            { 
            sb.AppendLine(outputItem.BaseObject.ToString());
            }

            //Clears the commands we added to the powershell runspace so it's empty the next time we use it
            ps.Commands.Clear();

            //If an error is encoutered, return it
            if (!string.IsNullOrEmpty(errorMsg)) 
                return errorMsg;

            return sb.ToString().Trim();

        }

    }
}
