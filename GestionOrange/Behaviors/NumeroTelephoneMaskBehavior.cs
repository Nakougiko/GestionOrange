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

        private void OnEntryTextChanged(object? sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender!;
            entry.Text = FormatPhoneNumber(e.NewTextValue);
            entry.CursorPosition = entry.Text.Length;
        }
        
        private string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber;

            var numericInput = new string(phoneNumber.Where(c => char.IsDigit(c)).ToArray());

            // Si le numéro est plus long que 2 chiffres, ajouter un espace après chaque paire de chiffres
            if (numericInput.Length > 2)
            {
                var formattedNumber = string.Empty;
                for (int i = 0; i < numericInput.Length; i++)
                {
                    if (i % 2 == 0 && i != 0)
                        formattedNumber += " " + numericInput[i];
                    else
                        formattedNumber += numericInput[i];
                }
                return formattedNumber;
            }
            else
            {
                return numericInput;
            }
        }   
    }
}
