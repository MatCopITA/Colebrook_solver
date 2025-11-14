using System;
using System.Threading;

class Program
{
  static void Main(string[] args)
  {
    Console.WriteLine("Enter the fluid velocity (m/s): ");
    string v_str = Console.ReadLine(); // Velocity in m/s
    Console.WriteLine("Enter the pipe diameter (m): ");
    string D_str = Console.ReadLine(); // Diameter in meters
    Console.WriteLine("Enter the pipe roughness (μm): ");
    string K_str = Console.ReadLine(); // Roughness in meters
    Console.WriteLine("Enter the fluid density (kg/m^3): ");
    string rho_str = Console.ReadLine(); // Density in kg/m^3
    Console.WriteLine("Enter the fluid dynamic viscosity (Pa.s): ");
    string mu_str = Console.ReadLine(); // Dynamic viscosity in Pa.s

    string Q_str = null; // Volumetric flow rate in m^3/s

    double v = Convert.ToDouble(v_str);
    double D = Convert.ToDouble(D_str);
    double K = Convert.ToDouble(K_str);
    double rho = Convert.ToDouble(rho_str);
    double mu = Convert.ToDouble(mu_str);

    double Re = (rho * v * D) / mu; // Reynolds number

    if (K == 0)
    {
      if (Re < 2000)
      {
        Console.WriteLine("f(Re) (Laminar, Smooth Pipe): " + (16 / Re));
      }
      else
      {
        Console.WriteLine("f(Re) (Turbulent, Smooth Pipe): " + (0.079 / Math.Pow(Re, -0.25)));
      }
    }
    else
    {
      double FindColebrook()
      {
        double f_local = 0.02;
        double fOld;
        int maxIterations = 10000;
        double tolerance = 1e-10;

        for (int i = 0; i < maxIterations; i++)
        {
          fOld = f_local;

          f_local = 1.0 / Math.Pow(-1.7 * Math.Log(((K*Math.Pow(10,-6)) / D) + (4.67 / (Re * Math.Sqrt(f_local))) + 2.28), 2);
          if (f_local <= 0) throw new InvalidOperationException("Non-physical friction factor computed.");

          if (Math.Abs(f_local - fOld) < tolerance) break;
        }
        return f_local;
      }
      double f = FindColebrook();
      Console.WriteLine("f(Re, K) (Turbulent, Rough Pipe): " + f);
    }
  }
}