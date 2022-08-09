

using System.Text;

namespace ConsoleApp2
{
	internal class Program
	{
		const int target = 931;
		static List<Dictionary<string, string>> solutions = new List<Dictionary<string, string>>();
		static void Main(string[] args)
		{
			var list = new List<int> { 2, 4, 3, 50, 8, 5 };
			Dictionary<string, string> output = new Dictionary<string, string>();
			System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
			stopWatch.Start();
			solve(list, output);
			stopWatch.Stop();			
			print_solutions(list);
			Console.WriteLine("Found " + solutions.Count + " solutions in " + stopWatch.Elapsed);
		}

        static void solve(List<int> input, Dictionary<string, string> operations)
		{
			input.Sort();
			check_solution(input, operations);
			StringBuilder result = new StringBuilder("");
			if (input.Count > 2)
			{
				List<string> pairs = new List<string>();
				for (int i = 0; i < input.Count - 1; i++)
				{
					for (int j = i + 1; j < input.Count; j++)
					{
						result.Append(input[i]).Append(",").Append(input[j]);
						if (!pairs.Contains(result.ToString()))
						{
							pairs.Add(result.ToString());
							List<int> newInput = get_combination(input, i, j);
							process_addition(new List<int>(newInput), input[i], input[j], operations);
							process_multiplication(new List<int>(newInput), input[i], input[j], operations);
							process_subtraction(new List<int>(newInput), input[i], input[j], operations);
							process_division(new List<int>(newInput), input[i], input[j], operations);
						}
					}
				}
			}
		}
		static void check_solution(List<int> input, Dictionary<string, string> operations)
		{
			for (int i = 0; i < input.Count; i++)
			{
				for (int j = i + 1; j < input.Count; j++)
				{
					var process = check_target(input[j], input[i], operations);
					if (process.Length > 0)
					{
						operations[process] = target.ToString();
						solutions.Add(operations);
					}
				}
			}
		}
		static string check_target(int x, int y, Dictionary<string, string> operations)
		{
			if (x + y == target)
			{
				StringBuilder result = new StringBuilder(x.ToString());
				return result.Append("+").Append(y).ToString();
			}
			if (x * y == target && y != 1)
			{
				StringBuilder result = new StringBuilder(x.ToString());
				return result.Append("*").Append(y).ToString();
			}
			if (x - y == target)
			{
				StringBuilder result = new StringBuilder(y.ToString());
				return result.Append("-").Append(x).ToString();
			}
			if (x % y == 0 && y != 1 && x / y == target)
			{
				StringBuilder result = new StringBuilder(x.ToString());
				return result.Append("/").Append(y).ToString();
			}
			return "";
		}
		static List<int> get_combination(List<int> input, int i, int j)
		{
			List<int> comb = new List<int>();
			int x, y, z;
			for (x = 0; x < i; x++)
				comb.Add(input[x]);
			for (y = i + 1; y < j; y++)
				comb.Add(input[y]);
			for (z = j + 1; z < input.Count; z++)
				comb.Add(input[z]);

			return comb;
		}
		static void process_addition(List<int> input, int process1, int process2, Dictionary<string, string> operations)
		{
			StringBuilder process = new StringBuilder(process1.ToString());
			process.Append("+").Append(process2).ToString();
			input.Add(process1 + process2);
			var dict = CloneDictionary(operations);
			dict[process.ToString()] = String.Join(',', input);
			solve(input, dict);
		}
		static void process_multiplication(List<int> input, int process1, int process2, Dictionary<string, string> operations)
		{
			if (process1 != 1)
			{
				StringBuilder process = new StringBuilder(process1.ToString());
				process.Append("*").Append(process2).ToString();
				input.Add(process1 * process2);
				var dict = CloneDictionary(operations);
				dict[process.ToString()] = String.Join(',', input);
				solve(input, dict);
			}
		}
		static void process_subtraction(List<int> input, int process1, int process2, Dictionary<string, string> operations)
		{
			if (process2 > process1) // no need to perform subtraction if they are equal
			{
				StringBuilder process = new StringBuilder(process2.ToString());
				process.Append("-").Append(process1).ToString();
				input.Add(process2 - process1);
				var dict = CloneDictionary(operations);
				dict[process.ToString()] = String.Join(',', input);
				solve(input, dict);
			}
		}

		static void process_division(List<int> input, int process1, int process2, Dictionary<string, string> operations)
		{
			if (process1 != 1 && process2 % process1 == 0)
			{
				StringBuilder process = new StringBuilder(process2.ToString());
				process.Append("/").Append(process1).ToString();
				input.Add(process2 / process1);
				var dict = CloneDictionary(operations);
				dict[process.ToString()] = String.Join(',', input);
				solve(input, dict);
			}
		}
		static void print_solutions(List<int> list)
		{
			foreach (var solution in solutions)
			{
				Console.WriteLine(String.Join(',', list));
				foreach (var operatn in solution)
				{
					Console.WriteLine(operatn.Key + " " + operatn.Value);
				}
				Console.WriteLine("--------------");
			}
		}
		static Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>
   (Dictionary<TKey, TValue> original) where TValue : ICloneable
		{
			Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
																	original.Comparer);
			foreach (KeyValuePair<TKey, TValue> entry in original)
			{
				ret.Add(entry.Key, (TValue)entry.Value.Clone());
			}
			return ret;
		}
	}
}