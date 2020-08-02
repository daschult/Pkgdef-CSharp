using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Pkgdef_CSharp
{
    internal class PkgdefClassifier : IClassifier
    {
        internal static class TypeNames
        {
            internal const string SubstitutionString = "Pkgdef Registry Key Base";
            internal const string RegistryKeyRelativePath = "Pkgdef Registry Key Relative Path";
            internal const string RegistryKeyDataItemName = "Pkgdef Registry Key Data Item Name";
            internal const string RegistryKeyDataItemNumberValue = "Pkgdef Registry Key Data Item Number Value";
            internal const string RegistryKeyDataItemStringValue = "Pkgdef Registry Key Data Item String Value";
            internal const string Comment = "PkgDef Comment";
        }

        internal static class TypeDefinitions
        {
            [Export]
            [Name(PkgdefClassifier.TypeNames.SubstitutionString)]
            public static ClassificationTypeDefinition SubstitutionString { get; set; }

            [Export]
            [Name(PkgdefClassifier.TypeNames.RegistryKeyRelativePath)]
            public static ClassificationTypeDefinition RegistryKeyRelativePath { get; set; }

            [Export]
            [Name(PkgdefClassifier.TypeNames.RegistryKeyDataItemName)]
            public static ClassificationTypeDefinition RegistryKeyDataItemName { get; set; }

            [Export]
            [Name(PkgdefClassifier.TypeNames.RegistryKeyDataItemNumberValue)]
            public static ClassificationTypeDefinition RegistryKeyDataItemNumberValue { get; set; }

            [Export]
            [Name(PkgdefClassifier.TypeNames.RegistryKeyDataItemStringValue)]
            public static ClassificationTypeDefinition RegistryKeyDataItemStringValue { get; set; }

            [Export]
            [Name(PkgdefClassifier.TypeNames.Comment)]
            public static ClassificationTypeDefinition Comment { get; set; }
        }

        internal static class FormatDefinitions
        {
            [Export(typeof(EditorFormatDefinition))]
            [ClassificationType(ClassificationTypeNames = PkgdefClassifier.TypeNames.SubstitutionString)]
            [Name(PkgdefClassifier.TypeNames.SubstitutionString)]
            [UserVisible(true)]
            internal sealed class SubstitutionStringFormatDefinition : ClassificationFormatDefinition
            {
                public SubstitutionStringFormatDefinition()
                {
                    this.DisplayName = PkgdefClassifier.TypeNames.SubstitutionString;
                    this.IsBold = true;
                    this.ForegroundColor = Colors.Aqua;
                }
            }

            [Export(typeof(EditorFormatDefinition))]
            [ClassificationType(ClassificationTypeNames = PkgdefClassifier.TypeNames.RegistryKeyRelativePath)]
            [Name(PkgdefClassifier.TypeNames.RegistryKeyRelativePath)]
            [UserVisible(true)]
            internal sealed class RegistryKeyRelativePathFormatDefinition : ClassificationFormatDefinition
            {
                public RegistryKeyRelativePathFormatDefinition()
                {
                    this.DisplayName = PkgdefClassifier.TypeNames.RegistryKeyRelativePath;
                    this.ForegroundColor = Colors.Blue;
                }
            }

            [Export(typeof(EditorFormatDefinition))]
            [ClassificationType(ClassificationTypeNames = PkgdefClassifier.TypeNames.RegistryKeyDataItemName)]
            [Name(PkgdefClassifier.TypeNames.RegistryKeyDataItemName)]
            [UserVisible(true)]
            internal sealed class RegistryKeyDataItemNameFormatDefinition : ClassificationFormatDefinition
            {
                public RegistryKeyDataItemNameFormatDefinition()
                {
                    this.DisplayName = PkgdefClassifier.TypeNames.RegistryKeyDataItemName;
                    this.ForegroundColor = Colors.Red;
                }
            }

            [Export(typeof(EditorFormatDefinition))]
            [ClassificationType(ClassificationTypeNames = PkgdefClassifier.TypeNames.RegistryKeyDataItemNumberValue)]
            [Name(PkgdefClassifier.TypeNames.RegistryKeyDataItemNumberValue)]
            [UserVisible(true)]
            internal sealed class RegistryKeyDataItemNumberValueFormatDefinition : ClassificationFormatDefinition
            {
                public RegistryKeyDataItemNumberValueFormatDefinition()
                {
                    this.DisplayName = PkgdefClassifier.TypeNames.RegistryKeyDataItemNumberValue;
                    this.ForegroundColor = Colors.Black;
                }
            }

            [Export(typeof(EditorFormatDefinition))]
            [ClassificationType(ClassificationTypeNames = PkgdefClassifier.TypeNames.RegistryKeyDataItemStringValue)]
            [Name(PkgdefClassifier.TypeNames.RegistryKeyDataItemStringValue)]
            [UserVisible(true)]
            internal sealed class RegistryKeyDataItemStringValueFormatDefinition : ClassificationFormatDefinition
            {
                public RegistryKeyDataItemStringValueFormatDefinition()
                {
                    this.DisplayName = PkgdefClassifier.TypeNames.RegistryKeyDataItemStringValue;
                    this.ForegroundColor = Colors.Red;
                }
            }

            [Export(typeof(EditorFormatDefinition))]
            [ClassificationType(ClassificationTypeNames = PkgdefClassifier.TypeNames.Comment)]
            [Name(PkgdefClassifier.TypeNames.Comment)]
            [UserVisible(true)]
            internal sealed class CommentFormatDefinition : ClassificationFormatDefinition
            {
                public CommentFormatDefinition()
                {
                    this.DisplayName = PkgdefClassifier.TypeNames.Comment;
                    this.IsItalic = true;
                    this.ForegroundColor = Colors.Green;
                }
            }
        }

        internal static class ContentType
        {
            public const string PkgdefContentTypeString = "Pkgdef";

            [Export(typeof(ContentTypeDefinition))]
            [Name(PkgdefContentTypeString)]
            [BaseDefinition("plaintext")]
            public static ContentTypeDefinition PkgdefContentType { get; set; }

            [Export(typeof(FileExtensionToContentTypeDefinition))]
            [ContentType(PkgdefContentTypeString)]
            [FileExtension(".pkgdef")]
            public static FileExtensionToContentTypeDefinition PkgdefFileExtension { get; set; }

            [Export(typeof(FileExtensionToContentTypeDefinition))]
            [ContentType(PkgdefContentTypeString)]
            [FileExtension(".pkgundef")]
            public static FileExtensionToContentTypeDefinition PkgundefFileExtension { get; set; }
        }

        [Export(typeof(IClassifierProvider))]
        [ContentType(PkgdefClassifier.ContentType.PkgdefContentTypeString)]
        [FileExtension(".pkgdef")]
        internal class Provider : IClassifierProvider
        {
            [Import]
            public IClassificationTypeRegistryService ClassificationRegistry { get; set; }

            /// <summary>
            /// Gets a classifier for the given text buffer.
            /// </summary>
            /// <param name="buffer">The <see cref="ITextBuffer"/> to classify.</param>
            /// <returns>A classifier for the text buffer, or null if the provider cannot do so in its current state.</returns>
            public IClassifier GetClassifier(ITextBuffer buffer)
            {
                return buffer.Properties.GetOrCreateSingletonProperty(() => new PkgdefClassifier(this.ClassificationRegistry));
            }
        }

        private readonly IClassificationType substitutionStringClassificationType;
        private readonly IClassificationType registryKeyRelativePathClassificationType;
        private readonly IClassificationType registryKeyDataItemNameClassificationType;
        private readonly IClassificationType registryKeyDataItemNumberValueClassificationType;
        private readonly IClassificationType registryKeyDataItemStringValueClassificationType;
        private readonly IClassificationType commentClassificationType;
        private readonly VersionedCache<ITextBuffer,int,IReadOnlyList<ClassificationSpan>> textSnapshotClassifications;

        internal PkgdefClassifier(IClassificationTypeRegistryService registry)
        {
            PreCondition.AssertNotNull(registry, nameof(registry));

            this.substitutionStringClassificationType = registry.GetClassificationType(PkgdefClassifier.TypeNames.SubstitutionString);
            this.registryKeyRelativePathClassificationType = registry.GetClassificationType(PkgdefClassifier.TypeNames.RegistryKeyRelativePath);
            this.registryKeyDataItemNameClassificationType = registry.GetClassificationType(PkgdefClassifier.TypeNames.RegistryKeyDataItemName);
            this.registryKeyDataItemNumberValueClassificationType = registry.GetClassificationType(PkgdefClassifier.TypeNames.RegistryKeyDataItemNumberValue);
            this.registryKeyDataItemStringValueClassificationType = registry.GetClassificationType(PkgdefClassifier.TypeNames.RegistryKeyDataItemStringValue);
            this.commentClassificationType = registry.GetClassificationType(PkgdefClassifier.TypeNames.Comment);

            this.textSnapshotClassifications = new VersionedCache<ITextBuffer, int, IReadOnlyList<ClassificationSpan>>();
        }

#pragma warning disable 67

        /// <summary>
        /// An event that occurs when the classification of a span of text has changed.
        /// </summary>
        /// <remarks>
        /// This event gets raised if a non-text change would affect the classification in some way,
        /// for example typing /* would cause the classification to change in C# without directly
        /// affecting the span.
        /// </remarks>
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

#pragma warning restore 67

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            PreCondition.AssertNotNull(span, nameof(span));

            ITextSnapshot textSnapshot = span.Snapshot;
            ITextBuffer textBuffer = textSnapshot.TextBuffer;
            ITextVersion textVersion = textSnapshot.Version;
            int textVersionNumber = textVersion.VersionNumber;

            if (!this.textSnapshotClassifications.TryGet(textBuffer, textVersionNumber, out IReadOnlyList<ClassificationSpan> classificationSpans))
            {
                PkgdefDocument parsedDocument = PkgdefDocument.Parse(textSnapshot.GetText());

                List<ClassificationSpan> spans = new List<ClassificationSpan>();
                foreach (PkgdefSegment segment in parsedDocument.GetSegments())
                {
                    switch (segment.GetSegmentType())
                    {
                        case PkgdefSegmentType.LineComment:
                            spans.Add(PkgdefClassifier.CreateClassificationSpan(textSnapshot, segment, this.commentClassificationType));
                            break;

                        case PkgdefSegmentType.RegistryKeyPath:
                            spans.Add(PkgdefClassifier.CreateClassificationSpan(textSnapshot, segment, this.registryKeyRelativePathClassificationType));
                            break;

                        case PkgdefSegmentType.RegistryKeyDataItem:
                            PkgdefRegistryKeyDataItemSegment registryKeyDataItemSegment = (PkgdefRegistryKeyDataItemSegment)segment;
                            spans.Add(PkgdefClassifier.CreateClassificationSpan(textSnapshot, registryKeyDataItemSegment.GetNameSegment(), this.registryKeyDataItemNameClassificationType));
                            break;
                    }
                }
                classificationSpans = spans;
                this.textSnapshotClassifications.Set(textBuffer, textVersionNumber, classificationSpans);
            }

            return classificationSpans
                .Where((ClassificationSpan classificationSpan) => classificationSpan.Span.IntersectsWith(span.Span))
                .ToList();
        }

        private static ClassificationSpan CreateClassificationSpan(ITextSnapshot textSnapshot, PkgdefSegment segment, IClassificationType classificationType)
        {
            SnapshotSpan segmentSnapshotSpan = new SnapshotSpan(textSnapshot, segment.GetStartIndex(), segment.GetLength());
            return new ClassificationSpan(segmentSnapshotSpan, classificationType);
        }
    }
}
