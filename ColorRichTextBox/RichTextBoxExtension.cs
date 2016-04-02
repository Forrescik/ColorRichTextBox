using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ColorRichTextBox
{
    public static class RichTextBoxExtension
    {
        public static string Text(this RichTextBox richTextBox)
        {
            TextRange content = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            return content.Text;
        }
    }
}
