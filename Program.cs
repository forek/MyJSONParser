using System;
using Parser;

namespace MyJSONParser {
  class Program {
    static void Main (string[] args) {
      var str = Token.Tokenize("{}");
      Console.WriteLine(str);
    }
  }
}