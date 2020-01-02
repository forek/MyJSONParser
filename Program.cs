using System;
using MyJSONParser.Tokenize;
using MyJSONParser.Core;

namespace MyJSONParser {
  class Program {
    static void Main(string[] args) {
      string str = "{ \"foo\": 123, \"bar\": [123], \"baz\": { \"foo\": true, \"arr\": [null, null, true, false, 563] } }";

      var tokens = Token.Tokenize(str);
      var tree = Parser.Parse(tokens);

      Console.WriteLine("\nResult:");
      Console.WriteLine(tree);
    }
  }
}