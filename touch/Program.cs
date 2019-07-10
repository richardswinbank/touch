using System;
using System.IO;

namespace touch
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        if (args.Length != 2)
          throw new TouchException("touch usage: touch <filename> <timestamp>");
        var filename = args[0];
        if (!File.Exists(filename))
          throw new TouchException("touch: file \"" + filename + "\" not found");

        DateTime mt = ParseTimestamp(args[1]);
        Console.WriteLine("Set last modified time of \"" + filename + "\" to " + mt.ToString("dd MMM yyyy HH:mm:ss") + "? (Y to apply)");
        if (Console.ReadKey(true).Key == ConsoleKey.Y)
        {
          File.SetLastWriteTime(filename, mt);
          Console.WriteLine("Timestamp updated");
          return;
        }
         Console.WriteLine("No changes made");
      }
      catch (TouchException e)
      {
        Console.WriteLine(e.Message);
        Environment.Exit(1);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.StackTrace);
        Environment.Exit(2);
      }
    }

    private static DateTime ParseTimestamp(string ts)
    {
      return new DateTime(
          ParseInt(ts, 0, 4)
        , ParseInt(ts, 4, 2)
        , ParseInt(ts, 6, 2)
        , ParseInt(ts, 8, 2)
        , ParseInt(ts, 10, 2)
        , ParseInt(ts, 12, 2)
        );
    }

    private static int ParseInt(string ts, int off, int len)
    {
      try
      {
        return int.Parse(ts.Substring(off, len));
      }
      catch (FormatException)
      {
        InvalidTimestamp(ts);
      }
      catch (ArgumentOutOfRangeException)
      {
        InvalidTimestamp(ts);
      }
      return -1;  // never reached
    }

    private static void InvalidTimestamp(string ts)
    {
      throw new TouchException("touch: invalid timestamp: " + ts);
    }
  }

  internal class TouchException : Exception
  {
    public TouchException(string message) : base(message)
    {
    }
  }
}
