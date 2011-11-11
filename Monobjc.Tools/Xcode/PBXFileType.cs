//
// This file is part of Monobjc, a .NET/Objective-C bridge
// Copyright (C) 2007-2011 - Laurent Etiemble
//
// Monobjc is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// Monobjc is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Monobjc.  If not, see <http://www.gnu.org/licenses/>.
//
using System.ComponentModel;

namespace Monobjc.Tools.Xcode
{
	/// <summary>
	/// </summary>
	public enum PBXFileType
	{
		/// <summary>
		/// </summary>
		None = 0,
		/// <summary>
		/// </summary>
		[Description("archive.ar")]
		archiveAr,
		/// <summary>
		/// </summary>
		[Description("archive.asdictionary")]
		ArchiveAsdictionary,
		/// <summary>
		/// </summary>
		[Description("archive.binhex")]
		ArchiveBinhex,
		/// <summary>
		/// </summary>
		[Description("archive.ear")]
		ArchiveEar,
		/// <summary>
		/// </summary>
		[Description("archive.gzip")]
		ArchiveGzip,
		/// <summary>
		/// </summary>
		[Description("archive.jar")]
		ArchiveJar,
		/// <summary>
		/// </summary>
		[Description("archive.macbinary")]
		ArchiveMacbinary,
		/// <summary>
		/// </summary>
		[Description("archive.ppob")]
		ArchivePpob,
		/// <summary>
		/// </summary>
		[Description("archive.rsrc")]
		ArchiveRsrc,
		/// <summary>
		/// </summary>
		[Description("archive.stuffit")]
		ArchiveStuffit,
		/// <summary>
		/// </summary>
		[Description("archive.tar")]
		ArchiveTar,
		/// <summary>
		/// </summary>
		[Description("archive.war")]
		ArchiveWar,
		/// <summary>
		/// </summary>
		[Description("archive.zip")]
		ArchiveZip,
		/// <summary>
		/// </summary>
		[Description("audio.aiff")]
		AudioAiff,
		/// <summary>
		/// </summary>
		[Description("audio.au")]
		AudioAu,
		/// <summary>
		/// </summary>
		[Description("audio.midi")]
		AudioMidi,
		/// <summary>
		/// </summary>
		[Description("audio.mp3")]
		AudioMp3,
		/// <summary>
		/// </summary>
		[Description("audio.wav")]
		AudioWav,
		/// <summary>
		/// </summary>
		[Description("compiled.mach-o")]
		CompiledMachO,
		/// <summary>
		/// </summary>
		[Description("compiled.mach-o.bundle")]
		CompiledMachOBundle,
		/// <summary>
		/// </summary>
		[Description("compiled.mach-o.corefile")]
		CompiledMachOCorefile,
		/// <summary>
		/// </summary>
		[Description("compiled.mach-o.dylib")]
		CompiledMachODylib,
		/// <summary>
		/// </summary>
		[Description("compiled.mach-o.fvmlib")]
		CompiledMachOFvmlib,
		/// <summary>
		/// </summary>
		[Description("compiled.mach-o.objfile")]
		CompiledMachOObjfile,
		/// <summary>
		/// </summary>
		[Description("compiled.mach-o.preload")]
		CompiledMachOPreload,
		/// <summary>
		/// </summary>
		[Description("file.bplist")]
		FileBplist,
		/// <summary>
		/// </summary>
		[Description("file.xib")]
		FileXib,
		/// <summary>
		/// </summary>
		[Description("image.bmp")]
		ImageBmp,
		/// <summary>
		/// </summary>
		[Description("image.gif")]
		ImageGif,
		/// <summary>
		/// </summary>
		[Description("image.icns")]
		ImageIcns,
		/// <summary>
		/// </summary>
		[Description("image.ico")]
		ImageIco,
		/// <summary>
		/// </summary>
		[Description("image.jpeg")]
		ImageJpeg,
		/// <summary>
		/// </summary>
		[Description("image.pdf")]
		ImagePdf,
		/// <summary>
		/// </summary>
		[Description("image.pict")]
		ImagePict,
		/// <summary>
		/// </summary>
		[Description("image.png")]
		ImagePng,
		/// <summary>
		/// </summary>
		[Description("image.tiff")]
		ImageTiff,
		/// <summary>
		/// </summary>
		[Description("pattern.proxy")]
		PatternProxy,
		/// <summary>
		/// </summary>
		[Description("sourcecode.ada")]
		SourcecodeAda,
		/// <summary>
		/// </summary>
		[Description("sourcecode.applescript")]
		SourcecodeApplescript,
		/// <summary>
		/// </summary>
		[Description("sourcecode.asm")]
		SourcecodeAsm,
		/// <summary>
		/// </summary>
		[Description("sourcecode.asm.asm")]
		SourcecodeAsmAsm,
		/// <summary>
		/// </summary>
		[Description("sourcecode.asm.llvm")]
		SourcecodeAsmLlvm,
		/// <summary>
		/// </summary>
		[Description("sourcecode.c")]
		SourcecodeC,
		/// <summary>
		/// </summary>
		[Description("sourcecode.c.c.preprocessed")]
		SourcecodeCCPreprocessed,
		/// <summary>
		/// </summary>
		[Description("sourcecode.c.h")]
		SourcecodeCH,
		/// <summary>
		/// </summary>
		[Description("sourcecode.c.objc")]
		SourcecodeCObjc,
		/// <summary>
		/// </summary>
		[Description("sourcecode.c.objc.preprocessed")]
		SourcecodeCObjcPreprocessed,
		/// <summary>
		/// </summary>
		[Description("sourcecode.cpp.cpp")]
		SourcecodeCppCpp,
		/// <summary>
		/// </summary>
		[Description("sourcecode.cpp.cpp.preprocessed")]
		SourcecodeCppCppPreprocessed,
		/// <summary>
		/// </summary>
		[Description("sourcecode.cpp.h")]
		SourcecodeCppH,
		/// <summary>
		/// </summary>
		[Description("sourcecode.cpp.objcpp")]
		SourcecodeCppObjcpp,
		/// <summary>
		/// </summary>
		[Description("sourcecode.cpp.objcpp.preprocessed")]
		SourcecodeCppObjcppPreprocessed,
		/// <summary>
		/// </summary>
		[Description("sourcecode.dtrace")]
		SourcecodeDtrace,
		/// <summary>
		/// </summary>
		[Description("sourcecode.exports")]
		SourcecodeExports,
		/// <summary>
		/// </summary>
		[Description("sourcecode.fortran")]
		SourcecodeFortran,
		/// <summary>
		/// </summary>
		[Description("sourcecode.fortran.f77")]
		SourcecodeFortranF77,
		/// <summary>
		/// </summary>
		[Description("sourcecode.fortran.f90")]
		SourcecodeFortranF90,
		/// <summary>
		/// </summary>
		[Description("sourcecode.glsl")]
		SourcecodeGlsl,
		/// <summary>
		/// </summary>
		[Description("sourcecode.jam")]
		SourcecodeJam,
		/// <summary>
		/// </summary>
		[Description("sourcecode.java")]
		SourcecodeJava,
		/// <summary>
		/// </summary>
		[Description("sourcecode.javascript")]
		SourcecodeJavascript,
		/// <summary>
		/// </summary>
		[Description("sourcecode.lex")]
		SourcecodeLex,
		/// <summary>
		/// </summary>
		[Description("sourcecode.make")]
		SourcecodeMake,
		/// <summary>
		/// </summary>
		[Description("sourcecode.mig")]
		SourcecodeMig,
		/// <summary>
		/// </summary>
		[Description("sourcecode.nasm")]
		SourcecodeNasm,
		/// <summary>
		/// </summary>
		[Description("sourcecode.opencl")]
		SourcecodeOpencl,
		/// <summary>
		/// </summary>
		[Description("sourcecode.pascal")]
		SourcecodePascal,
		/// <summary>
		/// </summary>
		[Description("sourcecode.rez")]
		SourcecodeRez,
		/// <summary>
		/// </summary>
		[Description("sourcecode.yacc")]
		SourcecodeYacc,
		/// <summary>
		/// </summary>
		[Description("text")]
		Text,
		/// <summary>
		/// </summary>
		[Description("text.css")]
		TextCss,
		/// <summary>
		/// </summary>
		[Description("text.html.documentation")]
		TextHtmlDocumentation,
		/// <summary>
		/// </summary>
		[Description("text.man")]
		TextMan,
		/// <summary>
		/// </summary>
		[Description("text.pbxproject")]
		TextPbxproject,
		/// <summary>
		/// </summary>
		[Description("text.plist")]
		TextPlist,
		/// <summary>
		/// </summary>
		[Description("text.plist.info")]
		TextPlistInfo,
		/// <summary>
		/// </summary>
		[Description("text.plist.scriptSuite")]
		TextPlistScriptSuite,
		/// <summary>
		/// </summary>
		[Description("text.plist.scriptTerminology")]
		TextPlistScriptTerminology,
		/// <summary>
		/// </summary>
		[Description("text.plist.strings")]
		TextPlistStrings,
		/// <summary>
		/// </summary>
		[Description("text.plist.xclangspec")]
		TextPlistXclangspec,
		/// <summary>
		/// </summary>
		[Description("text.plist.xcsynspec")]
		TextPlistXcsynspec,
		/// <summary>
		/// </summary>
		[Description("text.plist.xctxtmacro")]
		TextPlistXctxtmacro,
		/// <summary>
		/// </summary>
		[Description("text.plist.xml")]
		TextPlistXml,
		/// <summary>
		/// </summary>
		[Description("text.rtf")]
		TextRtf,
		/// <summary>
		/// </summary>
		[Description("text.script")]
		TextScript,
		/// <summary>
		/// </summary>
		[Description("text.script.csh")]
		TextScriptCsh,
		/// <summary>
		/// </summary>
		[Description("text.script.perl")]
		TextScriptPerl,
		/// <summary>
		/// </summary>
		[Description("text.script.php")]
		TextScriptPhp,
		/// <summary>
		/// </summary>
		[Description("text.script.python")]
		TextScriptPython,
		/// <summary>
		/// </summary>
		[Description("textScript.ruby")]
		TextScriptRuby,
		/// <summary>
		/// </summary>
		[Description("text.scriptSh")]
		TextScriptSh,
		/// <summary>
		/// </summary>
		[Description("text.script.worksheet")]
		TextScriptWorksheet,
		/// <summary>
		/// </summary>
		[Description("text.xcconfig")]
		TextXcconfig,
		/// <summary>
		/// </summary>
		[Description("text.xml")]
		TextXml,
		/// <summary>
		/// </summary>
		[Description("video.avi")]
		VideoAvi,
		/// <summary>
		/// </summary>
		[Description("video.mpeg")]
		VideoMpeg,
		/// <summary>
		/// </summary>
		[Description("video.quartz-composer")]
		VideoQuartzComposer,
		/// <summary>
		/// </summary>
		[Description("video.quicktime")]
		VideoQuicktime,
		/// <summary>
		/// </summary>
		[Description("wrapper.application")]
		WrapperApplication,
		/// <summary>
		/// </summary>
		[Description("wrapper.cfbundle")]
		WrapperCFBundle,
		/// <summary>
		/// </summary>
		[Description("wrapper.framework")]
		WrapperFramework,
		/// <summary>
		/// </summary>
		[Description("wrapper.pb-project")]
		WrapperPBProject,
	}
}