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
    public class SourceSpecificationSerialized
    {
        /// <summary>
        /// 
        /// </summary>
        public SourceSpecificationSerialized()
        {
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class SourceSpecification
        {

            private int sourceIdField;

            private string sourceNameField;

            private string sourceTypeField;

            private string domainField;

            private string filenameField;

            private string launchDateField;

            private string sourceSpecificationLastEditedField;

            private List<SourceSpecificationFacet> facetField = new List<SourceSpecificationFacet>();

            /// <remarks/>
            public int SourceId
            {
                get
                {
                    return this.sourceIdField;
                }
                set
                {
                    this.sourceIdField = value;
                }
            }

            /// <remarks/>
            public string SourceName
            {
                get
                {
                    return this.sourceNameField;
                }
                set
                {
                    this.sourceNameField = value;
                }
            }

            /// <remarks/>
            public string SourceType
            {
                get
                {
                    return this.sourceTypeField;
                }
                set
                {
                    this.sourceTypeField = value;
                }
            }

            /// <remarks/>
            public string Domain
            {
                get
                {
                    return this.domainField;
                }
                set
                {
                    this.domainField = value;
                }
            }

            /// <remarks/>
            public string Filename
            {
                get
                {
                    return this.filenameField;
                }
                set
                {
                    this.filenameField = value;
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
            [System.Xml.Serialization.XmlElementAttribute("Facet")]
            public List<SourceSpecificationFacet> Facet
            {
                get
                {
                    return this.facetField;
                }
                set
                {
                    this.facetField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class SourceSpecificationFacet
        {

            private string facetTypeField;

            private string authorField;

            private SourceSpecificationFacetFacetSpecification facetSpecificationField;

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
            public SourceSpecificationFacetFacetSpecification FacetSpecification
            {
                get
                {
                    return this.facetSpecificationField;
                }
                set
                {
                    this.facetSpecificationField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class SourceSpecificationFacetFacetSpecification
        {

            private string facetSpecificationLanguageField;

            private string facetSpecificationLinkField;

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
            public string FacetSpecificationLink
            {
                get
                {
                    return this.facetSpecificationLinkField;
                }
                set
                {
                    this.facetSpecificationLinkField = value;
                }
            }
        }
    }
}