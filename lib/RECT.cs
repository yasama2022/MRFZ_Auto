using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace lib
{ 
     


    /// <summary>Values to pass to the GetDCEx method.</summary>
    [Flags()]
     enum DeviceContextValues : uint
    {
        /// <summary>DCX_WINDOW: Returns a DC that corresponds to the window rectangle rather 
        /// than the client rectangle.</summary>
        Window = 0x00000001,
        /// <summary>DCX_CACHE: Returns a DC from the cache, rather than the OWNDC or CLASSDC 
        /// window. Essentially overrides CS_OWNDC and CS_CLASSDC.</summary>
        Cache = 0x00000002,
        /// <summary>DCX_NORESETATTRS: Does not reset the attributes of this DC to the 
        /// default attributes when this DC is released.</summary>
        NoResetAttrs = 0x00000004,
        /// <summary>DCX_CLIPCHILDREN: Excludes the visible regions of all child windows 
        /// below the window identified by hWnd.</summary>
        ClipChildren = 0x00000008,
        /// <summary>DCX_CLIPSIBLINGS: Excludes the visible regions of all sibling windows 
        /// above the window identified by hWnd.</summary>
        ClipSiblings = 0x00000010,
        /// <summary>DCX_PARENTCLIP: Uses the visible region of the parent window. The 
        /// parent's WS_CLIPCHILDREN and CS_PARENTDC style bits are ignored. The origin is 
        /// set to the upper-left corner of the window identified by hWnd.</summary>
        ParentClip = 0x00000020,
        /// <summary>DCX_EXCLUDERGN: The clipping region identified by hrgnClip is excluded 
        /// from the visible region of the returned DC.</summary>
        ExcludeRgn = 0x00000040,
        /// <summary>DCX_INTERSECTRGN: The clipping region identified by hrgnClip is 
        /// intersected with the visible region of the returned DC.</summary>
        IntersectRgn = 0x00000080,
        /// <summary>DCX_EXCLUDEUPDATE: Unknown...Undocumented</summary>
        ExcludeUpdate = 0x00000100,
        /// <summary>DCX_INTERSECTUPDATE: Unknown...Undocumented</summary>
        IntersectUpdate = 0x00000200,
        /// <summary>DCX_LOCKWINDOWUPDATE: Allows drawing even if there is a LockWindowUpdate 
        /// call in effect that would otherwise exclude this window. Used for drawing during 
        /// tracking.</summary>
        LockWindowUpdate = 0x00000400,
        /// <summary>DCX_USESTYLE: Undocumented, something related to WM_NCPAINT message.</summary>
        UseStyle = 0x00010000,
        /// <summary>DCX_VALIDATE When specified with DCX_INTERSECTUPDATE, causes the DC to 
        /// be completely validated. Using this function with both DCX_INTERSECTUPDATE and 
        /// DCX_VALIDATE is identical to using the BeginPaint function.</summary>
        Validate = 0x00200000,
    }

    /// <summary>
    ///     Specifies a raster-operation code. These codes define how the color data for the
    ///     source rectangle is to be combined with the color data for the destination
    ///     rectangle to achieve the final color.
    /// </summary>
    enum TernaryRasterOperations : uint
    {
        /// <summary>dest = source</summary>
        SRCCOPY = 0x00CC0020,
        /// <summary>dest = source OR dest</summary>
        SRCPAINT = 0x00EE0086,
        /// <summary>dest = source AND dest</summary>
        SRCAND = 0x008800C6,
        /// <summary>dest = source XOR dest</summary>
        SRCINVERT = 0x00660046,
        /// <summary>dest = source AND (NOT dest)</summary>
        SRCERASE = 0x00440328,
        /// <summary>dest = (NOT source)</summary>
        NOTSRCCOPY = 0x00330008,
        /// <summary>dest = (NOT src) AND (NOT dest)</summary>
        NOTSRCERASE = 0x001100A6,
        /// <summary>dest = (source AND pattern)</summary>
        MERGECOPY = 0x00C000CA,
        /// <summary>dest = (NOT source) OR dest</summary>
        MERGEPAINT = 0x00BB0226,
        /// <summary>dest = pattern</summary>
        PATCOPY = 0x00F00021,
        /// <summary>dest = DPSnoo</summary>
        PATPAINT = 0x00FB0A09,
        /// <summary>dest = pattern XOR dest</summary>
        PATINVERT = 0x005A0049,
        /// <summary>dest = (NOT dest)</summary>
        DSTINVERT = 0x00550009,
        /// <summary>dest = BLACK</summary>
        BLACKNESS = 0x00000042,
        /// <summary>dest = WHITE</summary>
        WHITENESS = 0x00FF0062,
        /// <summary>
        /// Capture window as seen on screen.  This includes layered windows 
        /// such as WPF windows with AllowsTransparency="true"
        /// </summary>
        CAPTUREBLT = 0x40000000
    }

}
