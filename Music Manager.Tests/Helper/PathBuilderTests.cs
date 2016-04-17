using System;
using System.Collections.Generic;
using Koopakiller.Apps.MusicManager.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable UnusedVariable

namespace Koopakiller.Apps.MusicManager.Tests.Helper
{
    [TestClass()]
    public class PathBuilderTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest_RootPathNull()
        {
            var pb = new PathBuilder(null, "");
        }
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest_PatternNull()
        {
            var pb = new PathBuilder("", null);
        }
        [TestMethod]
        public void ConstructorTest()
        {
            var pb = new PathBuilder("A", "B");
            Assert.AreEqual("A", pb.RootPath);
            Assert.AreNotEqual(null, pb.Replacements);
            Assert.AreEqual(0, pb.Replacements.Count);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void PatternTest_SetNull()
        {
            var pb = new PathBuilder("", "")
            {
                Pattern = null,
            };
        }
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void RootPathTest_SetNull()
        {
            var pb = new PathBuilder("", "")
            {
                RootPath = null,
            };
        }

        [TestMethod]
        public void BuildTest()
        {
            var pb = new PathBuilder(@"C:\Test", "");
            var result = pb.Build();
            Assert.AreEqual(@"C:\Test", result);
        }

        [TestMethod]
        public void BuildTest_WithPattern()
        {
            var pb = new PathBuilder(@"C:\Test", @"Test");
            var result = pb.Build();
            Assert.AreEqual(@"C:\Test\Test", result);
        }

        [TestMethod]
        public void BuildTest_WithPatternAndReplacements()
        {
            var pb = new PathBuilder(@"C:\Test", @"Test\<Title><Extension>")
            {
                Replacements =
                {
                    ["Extension"]=".mp3",
                    ["Title"]="My Song",
                },
            };
            var result = pb.Build();
            Assert.AreEqual(@"C:\Test\Test\My Song.mp3", result);
        }

        [TestMethod]
        public void BuildTest_WithPatternAndNullReplacements1()
        {
            var pb = new PathBuilder(@"C:\Test", @"<AlbumArtist|Performer>\<Title><Extension>")
            {
                Replacements =
                {
                    ["Extension"]=".mp3",
                    ["Title"]="My Song",
                    ["AlbumArtist"]=null,
                    ["Performer"]="My Performer"
                },
            };
            var result = pb.Build();
            Assert.AreEqual(@"C:\Test\My Performer\My Song.mp3", result);
        }

        [TestMethod]
        public void BuildTest_WithPatternAndNullReplacements2()
        {
            var pb = new PathBuilder(@"C:\Test", @"<AlbumArtist|Performer>\<Title><Extension>")
            {
                Replacements =
                {
                    ["Extension"]=".mp3",
                    ["Title"]="My Song",
                    ["AlbumArtist"]="My Artist",
                    ["Performer"]=null,
                },
            };
            var result = pb.Build();
            Assert.AreEqual(@"C:\Test\My Artist\My Song.mp3", result);
        }

        [TestMethod, ExpectedException(typeof(NullReferenceException))]
        public void BuildTest_WithPatternAndInvalidNullReplacements()
        {
            var pb = new PathBuilder(@"C:\Test", @"<AlbumArtist|Performer>\<Title><Extension>")
            {
                Replacements =
                {
                    ["Extension"]=".mp3",
                    ["Title"]="My Song",
                    ["AlbumArtist"]=null,
                    ["Performer"]=null,
                },
            };
            pb.Build();
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void BuildTest_InvalidPattern()
        {
            var pb = new PathBuilder(@"C:\Test", @"<NotExist>");
            var result = pb.Build();
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void BuildTest_InvalidPatternBecauseCase()
        {
            var pb = new PathBuilder(@"C:\Test", @"<test>")
            {
                Replacements =
                {
                    ["Test"]="something",
                },
            };
            pb.Build();
        }

        [TestMethod]
        public void BuildTest_EmptyRootPath()
        {
            var pb = new PathBuilder("", @"PatternPath");
            var result = pb.Build();
            Assert.AreEqual("PatternPath", result);
        }

        [TestMethod]
        public void BuildTest_InvalidRootPathCharacters()
        {
            var pb = new PathBuilder(@"C:\Test:", @"PatternPath");
            var result = pb.Build();
            Assert.AreEqual(@"C:\Test:PatternPath", result);
        }

        [TestMethod]
        public void BuildTest_InvalidPatternCharacters()
        {
            var pb = new PathBuilder(@"C:\Test", @":");
            var result = pb.Build();
            Assert.AreEqual(@"C:\Test\:", result);
        }

        [TestMethod]
        public void BuildTest_InvalidRootPath()
        {
            var pb = new PathBuilder(@"C:\Test", @"<Test>")
            {
                Replacements =
                {
                    ["Test"]=":",
                },
            };
            var result = pb.Build();
            Assert.AreEqual(@"C:\Test\:", result);
        }
    }
}