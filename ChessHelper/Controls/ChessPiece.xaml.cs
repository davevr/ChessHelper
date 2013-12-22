using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessHelper
{
    public class PieceLoc
    {
        public PieceLoc(int theRow, int theCol)
        {
            Row = theRow;
            Column = theCol;
        }

        public int Row {get; set;}
        public int Column {get; set;}

        public bool IsBlack
        {
            get
            {
                if (Row % 2 == 0)
                {
                    if (Column % 2 == 0)
                        return false;
                    else
                        return true;
                }
                else
                {
                    if (Column % 2 == 0)
                        return true;
                    else
                        return false;
                }
            }  
        }
    }

    /// <summary>
    /// Interaction logic for ChessPiece.xaml
    /// </summary>
    public partial class ChessPiece : UserControl
    {
        private int _row = 0;
        private int _col = 0;
        private bool _isBlack = false;
        private string pieceType = "bishop";
        private bool _selected = false;
        private static Brush HighlightBrush = new SolidColorBrush(Colors.Yellow);
        private static Brush TransparentBrush = new SolidColorBrush(Colors.Transparent);

        public ChessPiece()
        {
            InitializeComponent();
        }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                if (_selected)
                    Background = HighlightBrush;
                else
                    Background = TransparentBrush;
            }
        }

        public static ChessPiece CreatePiece(string pieceName, bool isBlack)
        {
            ChessPiece newPiece = new ChessPiece();
            string imageStr = "Chess_";

            newPiece._isBlack = isBlack;
            newPiece.pieceType = pieceName;
            
            switch (pieceName)
            {
                case "rook":
                    imageStr += "r";
                    break;
                case "knight":
                    imageStr += "n";
                    break;
                case "bishop":
                    imageStr += "b";
                    break;
                case "king":
                    imageStr += "k";
                    break;
                case "queen":
                    imageStr += "q";
                    break;
                case "pawn":
                    imageStr += "p";
                    break;
            }

            if (isBlack)
                imageStr += "d";
            else
                imageStr += "l";
            imageStr += "t60.png";

            newPiece.PieceImage.Source = new BitmapImage(new Uri("../Assets/" + imageStr, UriKind.Relative));

            return newPiece;
        }

        public void SetPieceLocation(int row, int col)
        {
            _row = row;
            _col = col;
            UpdatePieceLoc();
        }

        public int Row
        {
            get { return _row; }
            set
            {
                _row = value;
                UpdatePieceLoc();
            }
        }

        public bool IsBlack
        {
            get { return _isBlack; }
            
        }

        public int Column
        {
            get { return _col; }
            set
            {
                _col = value;
                UpdatePieceLoc();
            }
        }

        private void UpdatePieceLoc()
        {
            Grid.SetRow(this, Row);
            Grid.SetColumn(this, Column);
            if (this.Parent != null)
            {
                
            }
        }

        public List<PieceLoc> GetPossibleMoves()
        {
            List<PieceLoc> possibleMoves = null;
            switch (pieceType)
            {
                case "rook":
                    possibleMoves = GetRookMoves();
                    break;
                case "knight":
                    possibleMoves = GetKnightMoves();
                    break;
                case "bishop":
                    possibleMoves = GetBishopMoves();
                    break;
                case "king":
                    possibleMoves = GetKingMoves();
                    break;
                case "queen":
                    possibleMoves = GetQueenMoves();
                    break;
                case "pawn":
                    possibleMoves = GetPawnMoves();
                    break;
            }


            return possibleMoves;
        }

        private List<PieceLoc> GetPawnMoves()
        {
            List<PieceLoc> possibleMoves = new List<PieceLoc>();
            
            if (IsBlack)
            {
                possibleMoves.Add(new PieceLoc(Row + 1, Column - 1));
                possibleMoves.Add(new PieceLoc(Row + 1, Column + 1));
            }
            else
            {
                possibleMoves.Add(new PieceLoc(Row - 1, Column - 1));
                possibleMoves.Add(new PieceLoc(Row - 1, Column + 1));
            }
            

            return TrimMoveList(possibleMoves);
        }

        private List<PieceLoc> GetBishopMoves()
        {
            List<PieceLoc> possibleMoves = new List<PieceLoc>();
            int curRow = Row, curCol = Column;
            ChessPiece curPiece;
            PieceLoc curLoc;

            for (int i = 1; i < 8; i++ )
            {
                curRow++;
                curCol++;
                curLoc = new PieceLoc(curRow, curCol);
                curPiece = PieceAtLocation(curLoc);
                if (curPiece != null)
                {
                    if (curPiece.IsBlack != IsBlack)
                        possibleMoves.Add(curLoc);
                    break;
                }
                else
                 possibleMoves.Add(curLoc);
            }

            curRow = Row; curCol = Column;
            for (int i = 1; i < 8; i++)
            {
                curRow--;
                curCol++;
                curLoc = new PieceLoc(curRow, curCol);
                curPiece = PieceAtLocation(curLoc);
                if (curPiece != null)
                {
                    if (curPiece.IsBlack != IsBlack)
                        possibleMoves.Add(curLoc);
                    break;
                }
                else
                    possibleMoves.Add(curLoc);
            }

            curRow = Row; curCol = Column;
            for (int i = 1; i < 8; i++)
            {
                curRow++;
                curCol--;
                curLoc = new PieceLoc(curRow, curCol);
                curPiece = PieceAtLocation(curLoc);
                if (curPiece != null)
                {
                    if (curPiece.IsBlack != IsBlack)
                        possibleMoves.Add(curLoc);
                    break;
                }
                else
                    possibleMoves.Add(curLoc);
            }

            curRow = Row; curCol = Column;
            for (int i = 1; i < 8; i++)
            {
                curRow--;
                curCol--;
                curLoc = new PieceLoc(curRow, curCol);
                curPiece = PieceAtLocation(curLoc);
                if (curPiece != null)
                {
                    if (curPiece.IsBlack != IsBlack)
                        possibleMoves.Add(curLoc);
                    break;
                }
                else
                    possibleMoves.Add(curLoc);
            }



            return TrimMoveList(possibleMoves);
        }

        private List<PieceLoc> GetRookMoves()
        {
            List<PieceLoc> possibleMoves = new List<PieceLoc>();
            int curRow = Row, curCol = Column;
            ChessPiece curPiece;
            PieceLoc curLoc;

            for (int i = 1; i < 8; i++)
            {
                curRow++;
                curLoc = new PieceLoc(curRow, curCol);
                curPiece = PieceAtLocation(curLoc);
                if (curPiece != null)
                {
                    if (curPiece.IsBlack != IsBlack)
                        possibleMoves.Add(curLoc);
                    break;
                }
                else
                    possibleMoves.Add(curLoc);
            }

            curRow = Row; curCol = Column;
            for (int i = 1; i < 8; i++)
            {
                curRow--;
                curLoc = new PieceLoc(curRow, curCol);
                curPiece = PieceAtLocation(curLoc);
                if (curPiece != null)
                {
                    if (curPiece.IsBlack != IsBlack)
                        possibleMoves.Add(curLoc);
                    break;
                }
                else
                    possibleMoves.Add(curLoc);
            }

            curRow = Row; curCol = Column;
            for (int i = 1; i < 8; i++)
            {
                curCol++;
                curLoc = new PieceLoc(curRow, curCol);
                curPiece = PieceAtLocation(curLoc);
                if (curPiece != null)
                {
                    if (curPiece.IsBlack != IsBlack)
                        possibleMoves.Add(curLoc);
                    break;
                }
                else
                    possibleMoves.Add(curLoc);
            }

            curRow = Row; curCol = Column;
            for (int i = 1; i < 8; i++)
            {
                curCol--;
                curLoc = new PieceLoc(curRow, curCol);
                curPiece = PieceAtLocation(curLoc);
                if (curPiece != null)
                {
                    if (curPiece.IsBlack != IsBlack)
                        possibleMoves.Add(curLoc);
                    break;
                }
                else
                    possibleMoves.Add(curLoc);
            }

            return TrimMoveList(possibleMoves);
        }

        private ChessPiece PieceAtLocation(PieceLoc theLoc)
        {
            return ((MainWindow)Window.GetWindow(this)).PieceAtLocation(theLoc);
        }

        private List<PieceLoc> TrimMoveList(List<PieceLoc> theList)
        {
            List<PieceLoc> possibleMoves = new List<PieceLoc>();

            foreach (PieceLoc theLoc in theList)
            {
                if ((theLoc.Row >= 0) &&
                    (theLoc.Row < 8) &&
                    (theLoc.Column >= 0) &&
                    (theLoc.Column < 8))
                {
                    ChessPiece curPiece = PieceAtLocation(theLoc);
                    if ((curPiece == null) || (curPiece.IsBlack != this.IsBlack))
                    {
                        possibleMoves.Add(theLoc);
                    }
                   
                }
            }

            return possibleMoves;
        }


        private List<PieceLoc> GetKnightMoves()
        {
            List<PieceLoc> possibleMoves = new List<PieceLoc>();
            possibleMoves.Add(new PieceLoc(Row - 1, Column - 2));
            possibleMoves.Add(new PieceLoc(Row - 1, Column + 2));
            possibleMoves.Add(new PieceLoc(Row + 1, Column - 2));
            possibleMoves.Add(new PieceLoc(Row + 1, Column + 2));
            possibleMoves.Add(new PieceLoc(Row - 2, Column - 1));
            possibleMoves.Add(new PieceLoc(Row - 2, Column + 1));
            possibleMoves.Add(new PieceLoc(Row + 2, Column - 1));
            possibleMoves.Add(new PieceLoc(Row + 2, Column + 1));

            return TrimMoveList(possibleMoves);
        }

        private List<PieceLoc> GetKingMoves()
        {
            List<PieceLoc> possibleMoves = new List<PieceLoc>();
            possibleMoves.Add(new PieceLoc(Row - 1, Column - 1));
            possibleMoves.Add(new PieceLoc(Row - 1, Column + 1));
            possibleMoves.Add(new PieceLoc(Row + 1, Column - 1));
            possibleMoves.Add(new PieceLoc(Row + 1, Column + 1));
            possibleMoves.Add(new PieceLoc(Row - 1, Column));
            possibleMoves.Add(new PieceLoc(Row + 1, Column));
            possibleMoves.Add(new PieceLoc(Row, Column - 1));
            possibleMoves.Add(new PieceLoc(Row, Column + 1));

            return TrimMoveList(possibleMoves);
        }

        private List<PieceLoc> GetQueenMoves()
        {
            List<PieceLoc> rookMoves = GetRookMoves();
            List<PieceLoc> bishopMoves = GetBishopMoves();
            List<PieceLoc> possibleMoves = rookMoves.Union(bishopMoves).ToList();


            return possibleMoves;
        }

    }
}
