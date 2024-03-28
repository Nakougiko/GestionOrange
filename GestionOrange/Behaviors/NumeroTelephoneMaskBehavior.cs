using System.Text.RegularExpressions;

namespace GestionOrange.Behaviors
{
    public class NumeroTelephoneMaskBehavior : Behavior<Entry>
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

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            entry.Text = FormatPhoneNumber(e.NewTextValue);
        }
        
        private string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber;

            var numericInput = new string(phoneNumber.Where(c => char.IsDigit(c)).ToArray());

            if (numericInput.Length < 10)
                return Regex.Replace(numericInput, @"(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})", "$1 $2 $3 $4 $5");
            else
                return numericInput.Substring(0, 10);
        }   
    }
}
