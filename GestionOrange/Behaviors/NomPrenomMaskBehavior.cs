using System.Text.RegularExpressions;

namespace GestionOrange.Behaviors
{
    public class NomPrenomMaskBehavior : Behavior<Entry>
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
            entry.Text = FormatNomPrenom(e.NewTextValue);
        }
        
        private string FormatNomPrenom(string nomPrenom)
        {
            if (string.IsNullOrWhiteSpace(nomPrenom))
                return nomPrenom;

            nomPrenom = Regex.Replace(nomPrenom, @"[^a-zA-ZÀ-ÿ\- ]", "");
            return nomPrenom;
        }
    }
}
