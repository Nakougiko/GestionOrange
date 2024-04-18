using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionOrange.Behaviors
{
    public class LatLongMaskBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            base.OnAttachedTo(entry);
            entry.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                return;

            ((Entry)sender!).Text = new string(e.NewTextValue.Where(c => char.IsDigit(c) || c == '.' || c == ',' || c == '-').ToArray());
        }
    }
}
