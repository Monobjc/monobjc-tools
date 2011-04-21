namespace Mvp.Xml.XInclude
{
	internal struct FallbackState
	{
		//Fallback is being processed
		public bool Fallbacking;
		//xi:fallback element depth
		public int FallbackDepth;
		//Fallback processed flag
		public bool FallbackProcessed;
	}

	/// <summary>
	/// XIncludingReader state machine.    
	/// </summary>
	/// <author>Oleg Tkachenko, http://www.xmllab.net</author>
	internal enum XIncludingReaderState
	{
		//Default state
		Default,
		//xml:base attribute is being exposed
		ExposingXmlBaseAttr,
		//xml:base attribute value is being exposed
		ExposingXmlBaseAttrValue,
		//xml:lang attribute is being exposed
		ExposingXmlLangAttr,
		//xml:lang attribute value is being exposed
		ExposingXmlLangAttrValue
	}
}
