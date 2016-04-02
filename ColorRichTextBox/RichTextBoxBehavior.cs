using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace ColorRichTextBox
{
    public class RichTextBoxBehavior : Behavior<RichTextBox>
    {
        private const int MaxTextLength = 100;
        private int _changeIndex = 0;

        public string TextCounter
        {
            get { return (string)GetValue(TextCounterProperty); }
            set { SetValue(TextCounterProperty, value); }
        }

        public static readonly DependencyProperty TextCounterProperty =
            DependencyProperty.Register("TextCounter", typeof(string), typeof(RichTextBoxBehavior), new PropertyMetadata(string.Empty));

        protected override void OnAttached()
        {
            AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
            base.OnDetaching();
        }

        private void AssociatedObjectOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
            var richTextBox = sender as RichTextBox;
            if (richTextBox != null)
            {
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Black));
                if (MaxTextLength - richTextBox.Text().Length < 0)
                {
                    var enterCount = richTextBox.Text().Count(c => c == '\r');
                    int index = 98 - enterCount;
                    int length = _changeIndex += textChangedEventArgs.Changes.Count;
                    TextPointer start = GetPointerFromCharOffset(index, range.Start, richTextBox.Document);
                    TextPointer end = GetPointerFromCharOffset(length + index, range.End, richTextBox.Document);
                    if (start != null && end != null)
                    {
                        TextRange tr = new TextRange(start, end);
                        tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
                    }
                }
                TextCounter = richTextBox.Text().Length.ToString();
            }
            AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
        }

        private TextPointer GetPointerFromCharOffset(int charOffset, TextPointer startPointer, FlowDocument document)
        {
            TextPointer startNavigate = startPointer;
            if (charOffset == 0)
            {
                return startNavigate;
            }
            TextPointer nextPointer = startNavigate;
            var counter = 0;
            while (nextPointer != null && counter <= charOffset)
            {
                if (nextPointer.CompareTo(document.ContentEnd) == 0)
                {
                    return nextPointer;
                }
                nextPointer = nextPointer.GetNextInsertionPosition(LogicalDirection.Forward);
                counter++;
            }
            return nextPointer;
        }
    }
}
