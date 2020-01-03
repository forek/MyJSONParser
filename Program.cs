using System;
using System.Collections;
using MyJSONParser.Core;
using MyJSONParser.Tokenize;

namespace MyJSONParser {
  class Program {
    static void Main(string[] args) {
      string str = "{ \"foo\": true, \"bar\": [123], \"baz\": { \"foo\": true, \"arr\": [null, null, true, false, 563] } }";

      var tokens = Token.Tokenize(str);
      var tree = Parser.Parse(tokens);

      Console.WriteLine(tree);

      var result = Generator.Generate(tree) as Hashtable;
      var barArray = result["bar"] as ArrayList;

      Console.WriteLine("\nResult:");
      Console.WriteLine($"{result["foo"]} {barArray[0]}");
    }
  }
}