using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoleyMoley
{
    public static class Cursors
    {
       // private System.Windows.Input.Cursor PreviousCursor { get; set; }

        public static void CursorWait()
        {
         //   PreviousCursor = this.Cursor;
         //   this.Cursor = System.Windows.Input.Cursors.Wait;
        }

        public static  void CursorRestore()
        {
         //   this.Cursor = PreviousCursor;
        }


    }
}
