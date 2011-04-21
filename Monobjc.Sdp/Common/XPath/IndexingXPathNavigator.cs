#region using

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Reflection;

#endregion using

namespace Mvp.Xml.Common.XPath
{
	/// <summary>	
    /// <see cref="IndexingXPathNavigator"/> enables lazy or eager indexing of any XML store
	/// (<see cref="XmlDocument"/>, <see cref="XPathDocument"/> or any other <see cref="IXPathNavigable"/> XML store) thus
	/// providing an alternative way to select nodes using XSLT key() function directly from an index table 
	/// instead of searhing the XML tree. This allows drastically decrease selection time
	/// on preindexed selections.
	/// </summary>
	/// <remarks>
	/// <para>Author: Oleg Tkachenko, <a href="http://www.xmllab.net">http://www.xmllab.net</a>.</para>
	/// <para>Contributors: Daniel Cazzulino, <a href="http://clariusconsulting.net/kzu">blog</a></para>
	/// <para>See <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnxmlnet/html/XMLindexing.asp">"XML Indexing Part 1: XML IDs, XSLT Keys and IndexingXPathNavigator"</a> article for more info.</para>
	/// </remarks>    
	public class IndexingXPathNavigator : XPathNavigator
	{
		#region Fields & Ctor

		private XPathNavigator nav;
		private XPathNavigatorIndexManager manager;

		/// <summary>
		/// Creates IndexingXPathNavigator over specified XPathNavigator.
		/// </summary>
		/// <param name="navigator">Core XPathNavigator</param>
		public IndexingXPathNavigator(XPathNavigator navigator)
		{
			this.nav = navigator;
			manager = new XPathNavigatorIndexManager();
		}

		#endregion Fields & Ctor

		#region Public Methods

		/// <summary>
		/// Builds indexes according to defined keys.
		/// </summary>
		public void BuildIndexes()
		{
			manager.BuildIndexes();
		}

		/// <summary>
		/// Adds named key for use with key() function.
		/// </summary>
		/// <param name="keyName">The name of the key</param>
		/// <param name="match">XPath pattern, defining the nodes to which 
		/// this key is applicable</param>
		/// <param name="use">XPath expression used to determine 
		/// the value of the key for each matching node</param>
		public virtual void AddKey(string keyName, string match, string use)
		{
			KeyDef key = new KeyDef(nav, match, use);
			manager.AddKey(nav, keyName, key);
		}

		#endregion Public Methods

		#region Important Overrides

		/// <summary>
		/// Compiles XPath expressions using base XPathNavigator.Compile()
		/// method and set IndexingXsltContext instance to the result of 
		/// the compilation so compiled expressions support key() extension
		/// function.
		/// </summary>
		/// <param name="xpath">XPath expression to complile</param>
		/// <returns>Compiled XPath expression with augmented context 
		/// to support key() extension function</returns>
		public override XPathExpression Compile(string xpath)
		{
			XPathExpression expr = base.Compile(xpath);
			expr.SetContext(new IndexingXsltContext(manager, nav.NameTable));
			return expr;
		}

		/// <summary>
		/// Selects a node set using the specified XPath expression.
		/// </summary>
		/// <param name="xpath">A string representing an XPath expression</param>
		/// <returns>An XPathNodeIterator pointing to the selected node set</returns>
		public override XPathNodeIterator Select(string xpath)
		{
			XPathExpression expr = Compile(xpath);
			return base.Select(expr);
		}

		/// <summary>
		/// Creates new cloned version of the IndexingXPathNavigator.
		/// </summary>
		/// <returns>Cloned copy of the IndexingXPathNavigator</returns>
		public override XPathNavigator Clone()
		{
			return new IndexingXPathNavigator(nav.Clone());
		}

		#endregion Important Overrides

		#region XPathNavigator dummy overrides

		/// <summary>
		/// See <see cref="XPathNavigator.NodeType"/>.
		/// </summary>
		public override XPathNodeType NodeType
		{
			get { return nav.NodeType; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.LocalName"/>.
		/// </summary>
		public override string LocalName
		{
			get { return nav.LocalName; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.Name"/>.
		/// </summary>
		public override string Name
		{
			get { return nav.Name; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.NamespaceURI"/>.
		/// </summary>
		public override string NamespaceURI
		{
			get { return nav.NamespaceURI; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.Prefix"/>.
		/// </summary>
		public override string Prefix
		{
			get { return nav.Prefix; }
		}

		/// <summary>
		/// See <see cref="XPathItem.Value"/>.
		/// </summary>
		public override string Value
		{
			get { return nav.Value; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.BaseURI"/>.
		/// </summary>
		public override String BaseURI
		{
			get { return nav.BaseURI; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.IsEmptyElement"/>.
		/// </summary>
		public override bool IsEmptyElement
		{
			get { return nav.IsEmptyElement; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.XmlLang"/>.
		/// </summary>
		public override string XmlLang
		{
			get { return nav.XmlLang; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.NameTable"/>.
		/// </summary>
		public override XmlNameTable NameTable
		{
			get { return nav.NameTable; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.HasAttributes"/>.
		/// </summary>
		public override bool HasAttributes
		{
			get { return nav.HasAttributes; }
		}

		/// <summary>
		/// See <see cref="XPathNavigator.GetAttribute"/>.
		/// </summary>
		public override string GetAttribute(string localName, string namespaceURI)
		{
			return nav.GetAttribute(localName, namespaceURI);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToAttribute"/>.
		/// </summary>
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			return nav.MoveToAttribute(localName, namespaceURI);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToFirstAttribute"/>.
		/// </summary>
		public override bool MoveToFirstAttribute()
		{
			return nav.MoveToFirstAttribute();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToNextAttribute"/>.
		/// </summary>
		public override bool MoveToNextAttribute()
		{
			return nav.MoveToNextAttribute();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.GetNamespace"/>.
		/// </summary>
		public override string GetNamespace(string localname)
		{
			return nav.GetNamespace(localname);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToNamespace"/>.
		/// </summary>
		public override bool MoveToNamespace(string @namespace)
		{
			return nav.MoveToNamespace(@namespace);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToFirstNamespace(XPathNamespaceScope)"/>.
		/// </summary>
		public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{
			return nav.MoveToFirstNamespace(namespaceScope);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToNextNamespace(XPathNamespaceScope)"/>.
		/// </summary>
		public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			return nav.MoveToNextNamespace(namespaceScope);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToNext()"/>.
		/// </summary>
		public override bool MoveToNext()
		{
			return nav.MoveToNext();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToPrevious"/>.
		/// </summary>
		public override bool MoveToPrevious()
		{
			return nav.MoveToPrevious();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToFirst"/>.
		/// </summary>
		public override bool MoveToFirst()
		{
			return nav.MoveToFirst();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToFirstChild"/>.
		/// </summary>
		public override bool MoveToFirstChild()
		{
			return nav.MoveToFirstChild();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToParent"/>.
		/// </summary>
		public override bool MoveToParent()
		{
			return nav.MoveToParent();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToRoot"/>.
		/// </summary>
		public override void MoveToRoot()
		{
			nav.MoveToRoot();
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveTo"/>.
		/// </summary>
		public override bool MoveTo(XPathNavigator other)
		{
			return nav.MoveTo(other);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.MoveToId"/>.
		/// </summary>
		public override bool MoveToId(string id)
		{
			return nav.MoveToId(id);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.IsSamePosition"/>.
		/// </summary>
		public override bool IsSamePosition(XPathNavigator other)
		{
			return nav.IsSamePosition(other);
		}

		/// <summary>
		/// See <see cref="XPathNavigator.HasChildren"/>.
		/// </summary>
		public override bool HasChildren
		{
			get { return nav.HasChildren; }
		}
		#endregion

		#region Internal classes

		#region IndexingXsltContext

		/// <summary>
		/// XsltContext providing key() extension function.
		/// </summary>
		private class IndexingXsltContext : XsltContext
		{
			private KeyExtensionFunction keyFuncImpl;

			/// <summary>
			/// Creates new IndexingXsltContext.
			/// </summary>
			/// <param name="manager">Newly created IndexingXsltContext</param>
			/// <param name="nt">The name table to use.</param>
			public IndexingXsltContext(XPathNavigatorIndexManager manager, XmlNameTable nt)
				:
				base(nt as NameTable)
			{
				keyFuncImpl = new KeyExtensionFunction(manager);
			}

			/// <summary>
			/// Not applicable.
			/// </summary>    
			public override int CompareDocument(string baseUri, string nextbaseUri)
			{
				return 0;
			}

			/// <summary>
			/// Not applicable.
			/// </summary>    
			public override bool PreserveWhitespace(XPathNavigator node)
			{
				return true;
			}

			/// <summary>
			/// Not applicable.
			/// </summary>
			public override bool Whitespace
			{
				get { return true; }
			}

			/// <summary>
			/// No custom variables.
			/// </summary>    
			public override IXsltContextVariable ResolveVariable(string prefix, string name)
			{
				return null;
			}

			/// <summary>
			/// Resolves key() extension function.
			/// </summary>
			/// <param name="prefix">The prefix of the function as it appears in the XPath expression</param>
			/// <param name="name">The name of the function</param>
			/// <param name="argTypes">An array of argument types for the function being resolved</param>
			/// <returns>KeyExtentionFunction implementation for key() extension function and null
			/// for anything else.</returns>
			public override IXsltContextFunction ResolveFunction(string prefix, string name,
				XPathResultType[] argTypes)
			{
				if (prefix.Length == 0 && name == "key")
				{
					if (argTypes.Length != 2)
						throw new ArgumentException(Monobjc.Tools.Sdp.Properties.Resources.IndexingXPathNavigator_KeyWrongArguments);
					else if (argTypes[0] != XPathResultType.String)
						throw new ArgumentException(Monobjc.Tools.Sdp.Properties.Resources.IndexingXPathNavigator_KeyArgumentNotString);
					else
						return keyFuncImpl;
				}
				else
					return null;
			}
		}

		#endregion IndexingXsltContext

		#region KeyExtensionFunction

		/// <summary>
		/// key() extension function implementation.
		/// </summary>
		private class KeyExtensionFunction : IXsltContextFunction
		{
			private const int args = 2;
			private static readonly XPathResultType[] argTypes =
				new XPathResultType[] { XPathResultType.String, XPathResultType.Any };
			private XPathNavigatorIndexManager manager;

			/// <summary>
			/// Creates new KeyExtentionFunction object.
			/// </summary>
			/// <param name="manager">Index manager for accessing indexes</param>
			public KeyExtensionFunction(XPathNavigatorIndexManager manager)
			{
				this.manager = manager;
			}

			/// <summary>
			/// Gets the minimum number of arguments for the function.
			/// </summary>
			public int Minargs
			{
				get { return args; }
			}

			/// <summary>
			/// Gets the maximum number of arguments for the function. 
			/// </summary>
			public int Maxargs
			{
				get { return args; }
			}

			/// <summary>
			/// Gets the supplied XPath types for the function's argument list.
			/// </summary>
			public XPathResultType[] ArgTypes
			{
				get { return argTypes; }
			}

			/// <summary>
			/// Gets the XPathResultType representing the XPath type returned by the function.
			/// </summary>
			public XPathResultType ReturnType
			{
				get { return XPathResultType.NodeSet; }
			}

			/// <summary>
			/// Provides the method to invoke the function with the given arguments in the given context.
			/// </summary>
			/// <param name="xsltContext">Given XSLT context</param>
			/// <param name="args">Array of actual arguments</param>
			/// <param name="docContext">Context document</param>
			/// <returns></returns>
			public object Invoke(XsltContext xsltContext, object[] args,
				XPathNavigator docContext)
			{
				return manager.GetNodes((string)args[0], args[1]);
			}
		}

		#endregion KeyExtensionFunction

		#region KeyDef

		/// <summary>
		/// Compilable key definition.
		/// </summary>
		private class KeyDef
		{
			private string match, use;
			private XPathExpression matchExpr, useExpr;
			private XPathNavigator nav;

			/// <summary>
			/// Creates a key definition with specified 'match' and 'use' expressions.
			/// </summary>
			/// <param name="nav">XPathNavigator to compile XPath expressions</param>
			/// <param name="match">XPath pattern, defining the nodes to 
			/// which this key is applicable</param>
			/// <param name="use">XPath expression expression used to 
			/// determine the value of the key for each matching node.</param>
			public KeyDef(XPathNavigator nav, string match, string use)
			{
				this.nav = nav;
				this.match = match;
				this.use = use;
			}

			/// <summary>
			/// XPath pattern, defining the nodes to 
			/// which this key is applicable.
			/// </summary>
			public string Match
			{
				get { return match; }
				set { match = value; }
			}

			/// <summary>
			/// XPath expression expression used to 
			/// determine the value of the key for each matching node.
			/// </summary>
			public string Use
			{
				get { return use; }
				set { use = value; }
			}

			/// <summary>
			/// Compiled XPath pattern, defining the nodes to 
			/// which this key is applicable.
			/// </summary>
			public XPathExpression MatchExpr
			{
				get
				{
					if (matchExpr == null)
						matchExpr = nav.Compile(match);
					return matchExpr;
				}
			}

			/// <summary>
			/// Compiled XPath expression expression used to 
			/// determine the value of the key for each matching node.
			/// </summary>
			public XPathExpression UseExpr
			{
				get
				{
					if (useExpr == null)
						useExpr = nav.Compile(use);
					return useExpr;
				}
			}

			/// <summary>
			/// Compiles match and use expressions.
			/// </summary>
			public void Compile()
			{
				matchExpr = nav.Compile(match);
				useExpr = nav.Compile(use);
			}
		}

		#endregion KeyDef

		#region XPathNavigatorIndex

		/// <summary>
		/// Index table for XPathNavigator.
		/// </summary>
		private class XPathNavigatorIndex
		{
			private List<KeyDef> keys;
			private IDictionary<string, List<XPathNavigator>> index;

			/// <summary>
			/// Creates index over specified XPathNavigator.
			/// </summary>
			public XPathNavigatorIndex()
			{
				keys = new List<KeyDef>();
				index = new Dictionary<string, List<XPathNavigator>>();
			}

			/// <summary>
			/// Adds a key.
			/// </summary>
			/// <param name="key">Key definition</param>
			public void AddKey(KeyDef key)
			{
				keys.Add(key);
			}

			/// <summary>
			/// Returns indexed nodes by a key value.
			/// </summary>
			/// <param name="keyValue">Key value</param>    
			public XPathNodeIterator GetNodes(object keyValue)
			{
				//As per XSLT spec:
				//When the second argument to the key function is of type node-set, 
				//then the result is the union of the result of applying the key function 
				//to the string value of each of the nodes in the argument node-set. 
				//When the second argument to key is of any other type, the argument is 
				//converted to a string as if by a call to the string function; it 
				//returns a node-set containing the nodes in the same document as 
				//the context node that have a value for the named key equal to this string.      
				List<XPathNavigator> indexedNodes = null, tmpIndexedNodes;
				if (keyValue is XPathNodeIterator)
				{
					XPathNodeIterator nodes = keyValue as XPathNodeIterator;
					while (nodes.MoveNext())
					{

						if (index.TryGetValue(nodes.Current.Value, out tmpIndexedNodes))
						{
							if (indexedNodes == null)
								indexedNodes = new List<XPathNavigator>();
							indexedNodes.AddRange(tmpIndexedNodes);
						}
					}
				}
				else
				{
					index.TryGetValue(keyValue.ToString(), out indexedNodes);
				}
				if (indexedNodes == null)
					indexedNodes = new List<XPathNavigator>(0);

				return new XPathNavigatorIterator(indexedNodes);
			}

			/// <summary>
			/// Matches given node against "match" pattern and adds it to 
			/// the index table if the matching succeeded.
			/// </summary>
			/// <param name="node">Node to match</param>
			public void MatchNode(XPathNavigator node)
			{
				foreach (KeyDef keyDef in keys)
				{
					if (node.Matches(keyDef.MatchExpr))
					{
						//Ok, let's calculate key value(s). As per XSLT spec:
						//If the result is a node-set, then for each node in the node-set, 
						//the node that matches the pattern has a key of the specified name whose 
						//value is the string-value of the node in the node-set; otherwise, the result 
						//is converted to a string, and the node that matches the pattern has a 
						//key of the specified name with value equal to that string.        
						object key = node.Evaluate(keyDef.UseExpr);
						if (key is XPathNodeIterator)
						{
							XPathNodeIterator ni = (XPathNodeIterator)key;
							while (ni.MoveNext())
								AddNodeToIndex(node, ni.Current.Value);
						}
						else
						{
							AddNodeToIndex(node, key.ToString());
						}
					}
				}
			}

			/// <summary>
			/// Adds node to the index slot according to key value.
			/// </summary>
			/// <param name="node">Node to add to index</param>
			/// <param name="key">String key value</param>
			private void AddNodeToIndex(XPathNavigator node, string key)
			{
				//Get slot
				List<XPathNavigator> indexedNodes;
				if (!index.TryGetValue(key, out indexedNodes))
				{
					indexedNodes = new List<XPathNavigator>();
					index.Add(key, indexedNodes);
				}
				indexedNodes.Add(node.Clone());
			}
		}

		#endregion XPathNavigatorIndex

		#region XPathNavigatorIndexManager

		/// <summary>
		/// Index manager. Manages collection of named indexes.
		/// </summary>
		private class XPathNavigatorIndexManager
		{
			private IDictionary<string, XPathNavigatorIndex> indexes;
			private XPathNavigator nav;
			private bool indexed;

			/// <summary>
			/// Adds new key to the named index.
			/// </summary>
			/// <param name="nav">XPathNavigator over XML document to be indexed</param>
			/// <param name="indexName">Index name</param>
			/// <param name="key">Key definition</param>
			public void AddKey(XPathNavigator nav, string indexName, KeyDef key)
			{
				this.indexed = false;
				this.nav = nav;
				//Named indexes are stored in a hashtable.
				if (indexes == null)
					indexes = new Dictionary<string, XPathNavigatorIndex>();
				XPathNavigatorIndex index;
				if (!indexes.TryGetValue(indexName, out index))
				{
					index = new XPathNavigatorIndex();
					indexes.Add(indexName, index);
				}
				index.AddKey(key);
			}

			/// <summary>
			/// Builds indexes.
			/// </summary>
			public void BuildIndexes()
			{
				XPathNavigator doc = nav.Clone();
				//Walk through the all document nodes adding each one matching 
				//'match' expression to the index.
				doc.MoveToRoot();
				//Select all nodes but namespaces and attributes
				XPathNodeIterator ni = doc.SelectDescendants(XPathNodeType.All, true);
				while (ni.MoveNext())
				{
					if (ni.Current.NodeType == XPathNodeType.Element)
					{
                        XPathNavigator tempNav = ni.Current.Clone();
						//Processs namespace nodes
                        for (bool go = tempNav.MoveToFirstNamespace(); go; go = tempNav.MoveToNextNamespace())
						{
							foreach (XPathNavigatorIndex index in indexes.Values)
                                index.MatchNode(tempNav);
						}
						//ni.Current.MoveToParent();

                        tempNav = ni.Current.Clone();
						//process attributes
                        for (bool go = tempNav.MoveToFirstAttribute(); go; go = tempNav.MoveToNextAttribute())
						{
							foreach (XPathNavigatorIndex index in indexes.Values)
                                index.MatchNode(tempNav);
						}
						//ni.Current.MoveToParent();
					}

					foreach (XPathNavigatorIndex index in indexes.Values)
						index.MatchNode(ni.Current);
				}
				indexed = true;
			}

			/// <summary>
			/// Get indexed nodes by index name and key value.
			/// </summary>    
			/// <param name="indexName">Index name</param>
			/// <param name="value">Key value</param>
			/// <returns>Indexed nodes</returns>
			public XPathNodeIterator GetNodes(string indexName, object value)
			{
				if (!indexed)
					BuildIndexes();
				XPathNavigatorIndex index;
				indexes.TryGetValue(indexName, out index);
				return index == null ? null : index.GetNodes(value);
			}
		}

		#endregion XPathNavigatorIndexManager

		#endregion Internal classes
	}
}