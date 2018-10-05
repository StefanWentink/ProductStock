// some .cs file included in your project
using System.Resources;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ProductStock.Api")]
[assembly: InternalsVisibleTo("ProductStock.Test")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: NeutralResourcesLanguage("en")]