﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3615
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace XmlProc
{

    /// <summary>
    /// 
    /// </summary>
    public class SolutionSerialized
    {
        /// <summary>
        /// 
        /// </summary>
        public SolutionSerialized()
        {
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class LanguageSpecificSpecification
        {

            private string facetTypeField;

            private string facetSpecificationLanguageField;

            private string authorField;

            private string launchDateField;

            private string sourceSpecificationLastEditedField;

            private LanguageSpecificSpecificationFacetSpecificationData facetSpecificationDataField;

            /// <remarks/>
            public string FacetType
            {
                get
                {
                    return this.facetTypeField;
                }
                set
                {
                    this.facetTypeField = value;
                }
            }

            /// <remarks/>
            public string FacetSpecificationLanguage
            {
                get
                {
                    return this.facetSpecificationLanguageField;
                }
                set
                {
                    this.facetSpecificationLanguageField = value;
                }
            }

            /// <remarks/>
            public string Author
            {
                get
                {
                    return this.authorField;
                }
                set
                {
                    this.authorField = value;
                }
            }

            /// <remarks/>
            public string LaunchDate
            {
                get
                {
                    return this.launchDateField;
                }
                set
                {
                    this.launchDateField = value;
                }
            }

            /// <remarks/>
            public string SourceSpecificationLastEdited
            {
                get
                {
                    return this.sourceSpecificationLastEditedField;
                }
                set
                {
                    this.sourceSpecificationLastEditedField = value;
                }
            }

            /// <remarks/>
            public LanguageSpecificSpecificationFacetSpecificationData FacetSpecificationData
            {
                get
                {
                    return this.facetSpecificationDataField;
                }
                set
                {
                    this.facetSpecificationDataField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LanguageSpecificSpecificationFacetSpecificationData
        {

            private string contentField;

            private string miscellaneousField;

            /// <remarks/>
            public string Content
            {
                get
                {
                    return this.contentField;
                }
                set
                {
                    this.contentField = value;
                }
            }

            /// <remarks/>
            public string Miscellaneous
            {
                get
                {
                    return this.miscellaneousField;
                }
                set
                {
                    this.miscellaneousField = value;
                }
            }
        }
    }
}