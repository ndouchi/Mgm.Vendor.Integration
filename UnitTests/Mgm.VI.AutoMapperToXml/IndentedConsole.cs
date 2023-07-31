using System;
using System.IO;

namespace Mgm.VI.AutoMapperToXml
{
    class IndentedConsole : TextWriter
    {
        readonly TextWriter _oldConsole;
        bool _doIndent;

        public IndentedConsole()
        {
            IndentAmount = 2;
            Indent = 0;
            _oldConsole = Console.Out;
            _doIndent = true;
            Console.SetOut(this);
        }

        public int Indent { get; set; }

        public int IndentAmount { get; set; }

        public override void Write(char ch)
        {
            if (_doIndent)
            {
                _doIndent = false;
                for (var ix = 0; ix < Indent; ++ix) 
                    _oldConsole.Write(new string(' ', IndentAmount));
            }
            _oldConsole.Write(ch);
            if (ch == '\n') _doIndent = true;
        }

        public override System.Text.Encoding Encoding
        {
            get { return _oldConsole.Encoding; }
        }
    }
}
