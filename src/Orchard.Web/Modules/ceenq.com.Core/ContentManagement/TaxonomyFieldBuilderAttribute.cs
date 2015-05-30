namespace ceenq.com.Core.ContentManagement
{
    public class TaxonomyFieldBuilderAttribute : FieldBuilderAttribute
    {
        public TaxonomyFieldBuilderAttribute(
            string name, 
            string displayName, 
            SelectionMode selectionMode = SelectionMode.MultiSelection,
            string hint = "Select all {0} that apply.",
            bool required = false,
            bool autoComplete = true,
            bool allowCustomTerms = true
            )
            : base(name, displayName)
        {
            Settings.Add("TaxonomyFieldSettings.Taxonomy", Name);
            Settings.Add("TaxonomyFieldSettings.LeavesOnly", true.ToString());
            Settings.Add("TaxonomyFieldSettings.SingleChoice", (selectionMode == SelectionMode.SingleSelection).ToString());
            Settings.Add("TaxonomyFieldSettings.Hint", string.Format(hint, displayName.ToLowerInvariant()));
            Settings.Add("TaxonomyFieldSettings.Required", required.ToString());
            Settings.Add("TaxonomyFieldSettings.Autocomplete", autoComplete.ToString());
            Settings.Add("TaxonomyFieldSettings.AllowCustomTerms", allowCustomTerms.ToString());
        }

        public enum SelectionMode
        {
            SingleSelection,
            MultiSelection
        }
    }
}
