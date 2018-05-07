using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Net;


namespace YVY
{
    #region CLASS TimeOut

    /// <summary>
    /// タイムアウトタイマ関数群
    /// </summary>
    public static class TimeOut
    {
        /// <summary>
        /// 秒あたりのTickCount値
        /// </summary>
        public static UInt32 TIMENONE = 0;
        public static int TICK_PAR_SEC = 1000;
        public static int SECOND = TICK_PAR_SEC;


        /// <summary>
        /// タイムアウト設定
        /// </summary>
        /// <param name="msec">タイムアウト時間(*ms)</param>
        /// <returns>タイムアウト値</returns>
        public static UInt32 Set(int msec)
        {
            UInt32 tim;
            unchecked { tim= (UInt32)(TickCount + msec); }
            if (msec == TIMENONE) tim++;
            return tim;
        }
        /// <summary>
        /// 秒単位のタイムアウト設定
        /// </summary>
        /// <param name="sec">タイムアウト時間（*秒）</param>
        /// <returns>タイムアウト値</returns>
        public static UInt32 SetSEC(int sec)
        {
            return Set(SECOND * sec);
        }

        /// <summary>
        /// タイムアウト判定
        /// tim==0 で無条件にtrueをかえす
        /// </summary>
        /// <param name="tim">タイムアウト値</param>
        /// <returns>タイムアウトでtrue</returns>
        public static bool Chk(UInt32 tim)
        {
            // if (tim == TIMENONE) return true;
            return RemainingTime(tim) <= 0;
        }
        /// <summary>
        /// タイムアウト判定
        /// tim==0 で無条件にfalseをかえす
        /// </summary>
        /// <param name="tim">タイムアウト値</param>
        /// <returns>タイムアウトでtrue</returns>
        public static bool Chk2(UInt32 tim)
        {
            if (tim == TIMENONE) return false;
            return RemainingTime(tim) <= 0;
        }

        /// <summary>
        /// 1msのベースカウント（だいたい版）
        /// </summary>
        public static UInt32 TickCount
        {
            get { return (UInt32)System.Environment.TickCount; }
        }

        /// <summary>
        /// 1ms ベースカウント（厳密版）
        /// </summary>
        public static UInt32 TickCount2
        {
            get
            {
                unchecked
                {
                    return (UInt32)(DateTime.Now.Ticks / 10000);
                }
            }
        }


        /// <summary>
        /// 残時間の算出
        /// </summary>
        /// <param name="to">タイムアウト値</param>
        /// <returns>残時間 ０以下の場合は経過時間となる</returns>
        public static Int32 RemainingTime(UInt32 tim)
        {
            if (tim == TIMENONE) return 0;
            unchecked
            {
                return (Int32)(tim - TickCount);
            }
        }
        /// <summary>
        /// タイマ値の加算
        /// タイマ値 tim に時間 msec を加算する
        /// </summary>
        /// <param name="tim">タイマ値</param>
        /// <param name="msec">加算時間</param>
        /// <returns>結果のタイマ値</returns>
        public static UInt32 TimeAdd(UInt32 tim, int msec)
        {
            UInt32 t;
            unchecked { t= tim + (UInt32)msec; }
            if (t == TIMENONE) t++;
            return t;
        }

    }
    #endregion

    #region CLASS TimeOut2

    public class TimeOut2
    {
        public static long TIMENONE = 0;
        public static long TICK_PAR_SEC = 10000000;
        public static long SECOND = TICK_PAR_SEC;

        public static long TickCount { get { return DateTime.Now.Ticks; } }
        public static long Set(long tim)
        {
            return TickCount+tim;
        }
        public static long Set(TimeSpan tim)
        {
            return TickCount + tim.Ticks;
        }
        public static long SetMS(long tim)
        {
            return TickCount + ((SECOND/1000)*tim);
        }
        public static long SetSEC(int tim)
        {
            return TickCount + (SECOND * tim);
        }
        public static long RemainingTime(TimeSpan tim)
        {
            return tim.Ticks - TickCount;
        }
        public static long RemainingTime(long tim)
        {
            return tim - TickCount;
        }
        public static TimeSpan RemainingTimeSpan(TimeSpan tim)
        {
            return new TimeSpan(RemainingTime(tim));
        }
        public static TimeSpan RemainingTimeSpan(long tim)
        {
            return new TimeSpan(RemainingTime(tim));
        }
        public static bool Chk(TimeSpan tim)
        {
            return RemainingTime(tim) <= 0;
        }
        public static bool Chk(long tim)
        {
            if (tim == TIMENONE) return true;
            return RemainingTime(tim) <= 0;
        }
        public static bool Chk2(long tim)
        {
            if (tim == TIMENONE) return false;
            return RemainingTime(tim) <= 0;
        }
    }
    #endregion

    #region CLASS BFF

    /// <summary>
    /// バッファ操作関数群
    /// </summary>
    public static class BFF
    {
        public static FileStream OpenFileStream(string fname)
        {
            return new FileStream(fname, FileMode.Open, FileAccess.Read);
        }
       
        /// <summary>
        /// Big-Endian
        /// </summary>
        /// <param name="bf"></param>
        /// <param name="indx"></param>
        /// <returns></returns>
        public static int getWordB(byte[] bf, int indx)
        {
            return (bf[indx] << 8) | bf[indx + 1];
        }
        public static UInt32 getDWordB(byte[] bf, int indx)
        {
            return
            ((UInt32)getWordB(bf, indx) << 16) | (UInt32)getWordB(bf, indx + 2);
        }
        public static UInt32 get3WordB(byte[] bf, int indx)
        {
            return
            ((UInt32)getWordB(bf, indx) << 8) | (UInt32)bf[indx + 2];
        }



        public static int getWordB(Stream stm)
        {
            int res = stm.ReadByte()<<8;
            res += stm.ReadByte();
            return res;
        }
        public static UInt32 get3WordB(Stream stm)
        {
            UInt32 res = (UInt32)stm.ReadByte() << 16;
            res += (UInt32)getWordB(stm);
            return res;
        }
        public static UInt32 getDWordB(Stream stm)
        {
            UInt32 res = (UInt32)getWordB(stm)<<16;
            res += (UInt32)getWordB(stm);
            return res;
        }



        public static void setWordB(byte[] bf, int indx, int dt)
        {
            bf[indx] = (byte)(dt >> 8);
            bf[indx + 1] = (byte)dt;
        }
        public static void setDWordB(byte[] bf, int indx, UInt32 dt)
        {
            setWordB(bf, indx, (int)(dt >> 16));
            setWordB(bf, indx + 2, (int)dt);
        }
        public static void set3WordB(byte[] bf, int indx, UInt32 dt)
        {
            bf[indx] = (byte)(dt >> 16);
            setWordB(bf, indx + 1, (int)dt);
        }

        public static void setByte(Stream stm,byte dt)
        {
            stm.WriteByte(dt);
        }
        public static void setWordB(Stream stm, int dt)
        {
            stm.WriteByte((byte)(dt >> 8));
            stm.WriteByte((byte)dt);

        }
        public static void set3WordB(Stream stm, UInt32 dt)
        {
            stm.WriteByte((byte)(dt>>16));
            setWordB(stm,(int)dt);
        }
        public static void setDWordB(Stream stm, UInt32 dt)
        {
            setWordB(stm, (int)(dt >> 16));
            setWordB(stm, (int)dt);
        }

        /// <summary>
        /// Little-Endian
        /// </summary>
        /// <param name="bf"></param>
        /// <param name="indx"></param>
        /// <returns></returns>
        public static int getWordL(byte[] bf, int indx)
        {
            return (bf[indx+1] << 8) | bf[indx];
        }
        public static UInt32 getDWordL(byte[] bf, int indx)
        {
            return
            ((UInt32)getWordL(bf, indx+2) << 16) | (UInt32)getWordL(bf, indx);
        }
        public static UInt32 get3WordL(byte[] bf, int indx)
        {
            return
                (UInt32)getWordL(bf, indx)  | (UInt32)bf[indx + 2]<<16 ;
        }
        public static void setWordL(byte[] bf, int indx, int dt)
        {
            bf[indx] = (byte)dt;
            bf[indx + 1] = (byte)(dt>>8);
        }
        public static void setDWordL(byte[] bf, int indx, UInt32 dt)
        {
            setWordL(bf, indx, (int)dt);
            setWordL(bf, indx + 2, (int)(dt>>16));
        }
        public static void set3WordL(byte[] bf, int indx, UInt32 dt)
        {
            setWordL(bf, indx, (int)dt );
            bf[indx+2] = (byte)(dt >> 16);
        }
        public static void setWordL(Stream stm, int dt)
        {
            stm.WriteByte((byte)dt);
            stm.WriteByte((byte)(dt>>8));

        }
        public static void set3WordL(Stream stm, UInt32 dt)
        {
            setWordL(stm, (int)dt);
            stm.WriteByte((byte)(dt >> 16));
        }
        public static void setDWordL(Stream stm, UInt32 dt)
        {
            setWordL(stm, (int)dt);
            setWordL(stm, (int)(dt >> 16));
        }

    }
    #endregion

    #region CLASS EDIT


    public static class EDIT
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();

        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;

        const int WM_VSCROLL = 0x0115;
        const int SB_BOTTOM = 7;


        // 最終行にスクロール
        /// <summary>
        /// Editコントロールの最終行スクロール関数
        /// </summary>
        /// <param name="ed">Editコントロール</param>
        /// <param name="Off">最終行からのオフセット</param>
        public static void edBottomScroll(Control ed, int Off = 0)
        {
            PostMessage(ed.Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, (IntPtr)Off);
        }

        /// <summary>
        /// TextBoxの最終行に追加、スクロール、CRLFを付加
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="msg"></param>
        public static void edPuts(TextBoxBase ed, string msg)
        {
            ed.AppendText(msg + "\r\n");
            ed.ScrollToCaret();
            edBottomScroll(ed);
        }
        /// <summary>
        /// TextBoxの最終行に追加、スクロール、CRLFを付加しない
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="msg"></param>
        public static void edAppendText(TextBoxBase ed, string msg)
        {
            ed.AppendText(msg);
            ed.ScrollToCaret();
            edBottomScroll(ed);
        }

        // 以下のように、MouseDown イベントに記述することで
        // Form の移動を行えるようにする
        //
        //private void Form_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        YVY.EDIT.MoveForm(Handle);
        //    }
        //}
        /// <summary>
        /// Form の MouseDown イベントでフォーム移動させる
        /// </summary>
        /// <param name="Handle"></param>
        public static void MoveForm(IntPtr Handle)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
        }
        public static void MoveForm(Form frm, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm(frm.Handle);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm((sender as Control).Handle);
            }
        }

    }
    #endregion


    #region CLASS Dialogs

    /// <summary>
    /// 通常のメッセージダイアログ
    /// </summary>
    public static class Dialogs
    {
        /// <summary>
        /// エラーダイアログ
        /// OKボタンのみ
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static DialogResult Error(string text, string caption = "エラー")
        {
            return MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static DialogResult Error(Control parent, string text, string caption = "エラー")
        {
            if (parent == null) throw new ArgumentNullException("parent");
            DialogResult res =DialogResult.None;
            parent.Invoke((Action)delegate () { res=Error(text, caption); });
            return res;
        }

        public static DialogResult ErrorRetry(string text, string caption = "エラー")
        {
            return MessageBox.Show(text, caption, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
        }


        /// <summary>
        /// エラーダイアログ
        /// YES、NOボタン
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static DialogResult ErrorYESNO(string text, string caption = "エラー")
        {
            return MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Exception 通知用エラーダイアログ
        /// OKボタンのみ
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static DialogResult Exception(Exception ex, string text = "", string caption = "例外が発生しました")
        {
            string s = "";
#if Debug
            s = ex.ToString() + "\r\n";
#else
            s += ex.Message;
#endif
            if (!string.IsNullOrWhiteSpace(text)) s += "\r\n" + text;

            return Error(s, caption);
            //return MessageBox.Show(s, caption,
            //     MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult Exception(Control parent, Exception ex, string text = "", string caption = "例外が発生しました")
        {
            DialogResult res = DialogResult.None;
            parent.Invoke((Action)delegate () { res = Exception(ex, text, caption); });
            return res;

        }
        /// <summary>
        /// 通知用ダイアログ
        /// OKボタンのみ
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static DialogResult Attension(string text, string caption = "通知")
        {
            return MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        public static DialogResult Attension(Control parent, string text, string caption = "通知")
        {
            DialogResult res = DialogResult.None;
            parent.Invoke((Action)delegate () { res = Attension( text, caption); });
            return res;
        }

        /// <summary>
        /// 問い合わせ用ダイアログ
        /// Yes,No ボタン
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static DialogResult Question(string text, string caption = "問い合わせ")
        {
            return MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        public static DialogResult Question(Control parent, string text, string caption = "問い合わせ")
        {
            DialogResult res = DialogResult.None;
            parent.Invoke((Action)delegate () { res = Question( text, caption); });
            return res;
        }


        public static DialogResult YESNO(string text, string caption = "問い合わせ")
        {
            return Question(text, caption);
        }

        /// <summary>
        /// 問い合わせ用ダイアログ
        /// Yes,No,Cancel ボタン
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static DialogResult YESNOCANCEL(string text, string caption = "問い合わせ")
        {
            return MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 問い合わせ用ダイアログ
        /// OK,Cancel ボタン
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static DialogResult OKCANCEL(string text, string caption = "問い合わせ")
        {
            return MessageBox.Show(text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 問い合わせ用ダイアログ
        /// Retry,Cancel ボタン
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static DialogResult RETRYCANCEL(string text, string caption = "問い合わせ")
        {
            return MessageBox.Show(text, caption, MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
        }
    }

    #endregion


    #region class IP
    public static class IP
    {
        public static IPEndPoint CreateEndPoint(string ip, int port, int ofst =0)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                return new IPEndPoint(IPAddress.Any, port);
            }

            byte[] bf = IPAddress.Parse(ip).GetAddressBytes();
            UInt32 p = BFF.getDWordB(bf, 0);
            p += (UInt32)ofst;
            BFF.setDWordB(bf, 0, p);

            return new IPEndPoint(new IPAddress(bf), port);
        }

        /// <summary>
        /// IP 文字列を返す
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="ofst"></param>
        /// <returns></returns>
        public static string toIPstr(string ip,int ofst)
        {
            if (string.IsNullOrWhiteSpace(ip)) return "---";
            
            byte[] bf = IPAddress.Parse(ip).GetAddressBytes();
            UInt32 p = BFF.getDWordB(bf, 0);
            p += (UInt32)ofst;
            BFF.setDWordB(bf, 0, p);

            return new IPAddress(bf).ToString();

        }

    }
    #endregion

    #region class Files

    /// <summary>
    /// ログ出力用
    /// </summary>
    public static class Files
    {

        public static Encoding DefEncoding = Encoding.UTF8;

        /// <summary>
        /// フォルダない場合は作成して追加
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool AppendAllText(string fname, string msg)
        {
            if (string.IsNullOrWhiteSpace(fname)) return false;

            try
            {
                File.AppendAllText(fname, msg, DefEncoding);
                return true;
            }
            catch { }

            try
            {
                string dir = Path.GetDirectoryName(fname);
                if (!Directory.Exists(dir))
                {
                    // フォルダがない場合は作る
                    Directory.CreateDirectory(dir);
                    File.AppendAllText(fname, msg, DefEncoding);
                    return true;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// ログの追加
        /// </summary>
        /// <param name="fname">ログファイル名</param>
        /// <param name="msg">追加ログ</param>
        /// <param name="title">ログファイルの先頭行に格納するタイトル</param>
        /// <returns></returns>
        public static bool AppendLogFile(string fname, string msg, string title = "")
        {
            if (string.IsNullOrWhiteSpace(fname)) return false;
            if (!File.Exists(fname) && !string.IsNullOrEmpty(title))
            {
                if (!AppendAllText(fname, title)) return false;
            }
            return AppendAllText(fname, msg);

        }

    }

    #endregion


}
