using System;

class Program
{
  static void Main(string[] args)
  {
    Console.Clear();

    string title = "\x1b[1m- Solver for Reynolds Number (Re) and Friction Factor (f(Re, K)) -\x1b[0m";
    Console.WriteLine($"{Environment.NewLine}");
    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (title.Length / 2)) + "}", title));

    Console.WriteLine();
    Console.WriteLine("Enter the fluid velocity (m/s): ");
    string? v_str = Console.ReadLine(); // Velocity in m/s

    Console.WriteLine("Enter the pipe diameter (m): ");
    string? D_str = Console.ReadLine(); // Diameter in meters

    Console.WriteLine("Enter the pipe roughness (μm): (0 if smooth)");
    string? K_str = Console.ReadLine(); // Roughness in meters 

    Console.WriteLine("Enter the fluid density (kg/m^3): ");
    string? rho_str = Console.ReadLine(); // Density in kg/m^3

    Console.WriteLine("Enter the fluid dynamic viscosity (Pa*s): ");
    string? mu_str = Console.ReadLine(); // Dynamic viscosity in Pa.s

    if (v_str == null || D_str == null || K_str == null || rho_str == null || mu_str == null)
    {
      Console.WriteLine("Invalid input. Please provide all required values.");
      return;
    }

    double v = Convert.ToDouble(v_str);
    double D = Convert.ToDouble(D_str);
    double K = Convert.ToDouble(K_str);
    double rho = Convert.ToDouble(rho_str);
    double mu = Convert.ToDouble(mu_str);

    double Re = (rho * v * D) / mu; // Reynolds number

    Console.WriteLine("Reynolds number (Re): " + Re);

    if (K == 0)
    {
      if (Re < 2100)
      {
        Console.WriteLine("f(Re) (Laminar, Smooth Pipe): " + (16 / Re));
      }
      else
      {
        Console.WriteLine("f(Re) (Turbulent, Smooth Pipe): " + (0.079 * Math.Pow(Re, -0.25))); //Blasius formula
      }
    }
    else
    {
      if (Re < 2100)
      {
        Console.WriteLine("f(Re, K) (Laminar, Rough Pipe): " + (16 / Re));
      }
      else
      {
        double FindColebrook()
        {
          double f_local = 1e-10;
          double f_old;
          double maxIter = 1e10;
          double tolerance = 1e-10;

          for (int i = 0; i < maxIter; i++)
          {
            f_old = f_local;

            f_local = 1.0 / Math.Pow(-1.7 * Math.Log(((K * Math.Pow(10, -6)) / D) + (4.67 / (Re * Math.Sqrt(f_local))) + 2.28), 2); //Colebrook formula

            if (f_local <= 0) throw new InvalidOperationException("Non-physical friction factor computed.");

            if (Math.Abs(f_local - f_old) < tolerance) break;
          }
          return f_local;
        }

        double f = FindColebrook();
        Console.WriteLine("f(Re, K) (Turbulent, Rough Pipe): " + f);
      }
    }
    Console.WriteLine($"{Environment.NewLine}Press any key to reset");
    Console.ReadKey();
    Main(args);
  }
}