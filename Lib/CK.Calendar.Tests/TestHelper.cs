using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;


namespace CK.Calendar.Tests
{
    class TestHelper
    {
        static string _testFolder;
        static string _solutionFolder;

        static IActivityMonitor _monitor;
		static ActivityMonitorConsoleClient _console;
		static ActivityMonitorTextWriterClient _log;
        static TestHelper()
        {
            _monitor = new ActivityMonitor();
            _monitor.Output.BridgeTarget.HonorMonitorFilter = false;
			Action<string> _logAction = Log_To_File;
			_log = new ActivityMonitorTextWriterClient(_logAction);
			_console = new ActivityMonitorConsoleClient();
            _monitor.Output.RegisterClients( _console,_log);
        }

		public static void Log_To_File(string msg)
		{
			DateTimeOffset _date = DateTimeOffset.Now;
			string _nameFile = _date.Day.ToString() + "_" + _date.Month.ToString() + "_" + _date.Year.ToString() + " Calendar.log";
			msg = Environment.NewLine + _date.ToString() + Environment.NewLine + msg;
			File.AppendAllText(Path.Combine(LogFolder,_nameFile), msg);
		}

        public static IActivityMonitor ConsoleMonitor
        {
            get { return _monitor; }
        }

        public static bool LogsToConsole
        {
            get { return _monitor.Output.Clients.Contains( _log ); }
            set
            {
                if( value ) _monitor.Output.RegisterUniqueClient( c => c == _log, () => _log );
                else _monitor.Output.UnregisterClient( _log );
            }
        }

        public static string TestFolder
        {
            get
            {
                if( _testFolder == null ) InitalizePaths();
                return _testFolder;
            }
        }
        
        public static string SolutionFolder
        {
            get
            {
                if( _solutionFolder == null ) InitalizePaths();
                return _solutionFolder;
            }
        }

        public static string CacheFolder
        {
            get
            {
                return Path.Combine( SolutionFolder, "CachedData" );
            }
        }

		public static string LogFolder
		{
			get
			{
				return Path.Combine(CacheFolder, "Logs");
			}
		}

        public static void CleanupTestFolder()
        {
            DeleteFolder( TestFolder,false);
        }

        public static void DeleteFolder( string directoryPath, bool recreate = false )
        {
			
            int tryCount = 0;
            for( ; ; )
            {
                try
                {
					if (Directory.Exists(directoryPath))
					{
						Directory.Delete(directoryPath, true);
					}
                    if( recreate )
                    {
                        Directory.CreateDirectory( directoryPath );
                        File.WriteAllText( Path.Combine( directoryPath, "TestWrite.txt" ), "Test write works." );
                        File.Delete( Path.Combine( directoryPath, "TestWrite.txt" ) );
                    }
                    return;
                }
                catch( Exception ex )
                {
                    if( ++tryCount == 20 ) throw;
                    ConsoleMonitor.Info().Send( ex, "While cleaning up directory '{0}'. Retrying.", directoryPath );
                    System.Threading.Thread.Sleep( 100 );
                }
            }
        }

        private static void InitalizePaths()
        {
            string p = new Uri( System.Reflection.Assembly.GetExecutingAssembly().CodeBase ).LocalPath;
            // => CK.XXX.Tests/bin/Debug/
            p = Path.GetDirectoryName( p );
            // => CK.XXX.Tests/bin/
            p = Path.GetDirectoryName( p );
            // => CK.XXX.Tests/
            p = Path.GetDirectoryName( p );
            // ==> CK.XXX.Tests/TestDir
            _testFolder = Path.Combine( p, "TestDir" );
            do
            {
                p = Path.GetDirectoryName( p );
            }
            while( !Directory.Exists( Path.Combine( p, "RSSFluxSD" ) ) );
            _solutionFolder = p;

            ConsoleMonitor.Info().Send( "SolutionFolder is: {1}\r\nTestFolder is: {0}", _testFolder, _solutionFolder );
            CleanupTestFolder();
            Directory.CreateDirectory( CacheFolder );
			Directory.CreateDirectory(LogFolder);
        }

    }
}
