using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GestionOrange.Behaviors
{
    public class AdresseMacMaskBehavior : Behavior<Entry>
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
            var entry = (Entry)sender!;
            var text = e.NewTextValue;

            if (string.IsNullOrWhiteSpace(text))
                return;

            // On retire tous les caractères non héxadécimaux
            text = Regex.Replace(text, @"[^0-9A-Fa-f]", string.Empty);
            text = text.ToUpper();

            // Insérer : après 2 caractères
            text = Regex.Replace(text, @"(.{2})(?=(.{2})+(?!.))", "$1:");

            // (XX:XX:XX:XX:XX:XX) 17 caractères
            if (text.Length > 17)
            {
                text = text.Substring(0, 17);
            }

            entry.Text = text;
        }
    }
}
