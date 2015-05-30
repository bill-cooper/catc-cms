using System;
using System.Collections.Generic;
using Orchard.Taxonomies.Services;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions.Models;
using ceenq.com.Core.Environment;
using ceenq.com.Core.Utility;

namespace ceenq.org.Resource.Initialization
{
    public class BookListDataInitializer : TaxonomyDataInitializer
    {
        public BookListDataInitializer(ITaxonomyService taxonomyService, IContentManager contentManager)
            : base(taxonomyService, contentManager) { }

        protected override ExtensionDescriptor ContainerExtension
        {
            get { return ModuleUtility.ContainerExtentionFor<BookListDataInitializer>(); }
        }

        protected override string TaxonomyName
        {
            get { return "Book"; }
        }

        protected override Lazy<List<string>> Terms
        {
            get
            {
                return new Lazy<List<string>>(() => new List<string>(){
                "Genesis",
                "Exodus",
                "Leviticus",
                "Numbers",
                "Deuteronomy",
                "Joshua",
                "Judges",
                "Ruth",
                "1 Samuel",
                "2 Samuel",
                "1 Kings",
                "2 Kings",
                "1 Chronicles",
                "2 Chronicles",
                "Ezra",
                "Nehemiah",
                "Esther",
                "Job",
                "Psalms",
                "Proverbs",
                "Ecclesiastes",
                "Song of Solomon",
                "Isaiah",
                "Jeremiah",
                "Lamentations",
                "Ezekiel",
                "Daniel",
                "Hosea",
                "Joel",
                "Amos",
                "Obadiah",
                "Jonah",
                "Micah",
                "Nahum",
                "Habakkuk",
                "Zephaniah",
                "Haggai",
                "Zechariah",
                "Malachi",
                "Matthew",
                "Mark",
                "Luke",
                "John",
                "Acts",
                "Romans",
                "1 Corinthians",
                "2 Corinthians",
                "Galatians",
                "Ephesians",
                "Philippians",
                "Colossians",
                "1 Thessalonians",
                "2 Thessalonians",
                "1 Timothy",
                "2 Timothy",
                "Titus",
                "Philemon",
                "Hebrews",
                "James",
                "1 Peter",
                "2 Peter",
                "1 John",
                "2 John",
                "3 John",
                "Jude",
                "Revelation"
            });
            }
        }


    }
}