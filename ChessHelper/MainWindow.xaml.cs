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

    public class ThreatCounter
    {
        static private Brush TransparentBrush = new SolidColorBrush(Colors.Transparent);
        private int _whiteCount = 0;
        private int _blackCount = 0;

        public Rectangle Marker { get; set; }

        public ThreatCounter(Rectangle theRect)
        {
            Marker = theRect;
            WhiteCount = 0;
            BlackCount = 0;
            RecomputeColor();
        }

        public void Clear()
        {
            WhiteCount = 0;
            BlackCount = 0;
            RecomputeColor();
        }

        public int WhiteCount 
        { 
            get {return _whiteCount;}
            set
            {
                _whiteCount = value;
                RecomputeColor();
            }
        }
        public int BlackCount
        {
            get {return _blackCount;}
            set
            {
                _blackCount = value;
                RecomputeColor();
            }
        }

        public void RecomputeColor()
        {
            if ((_whiteCount == 0) && (_blackCount == 0))
            {
                Marker.Fill = TransparentBrush;
            }
            else
            {
                int redPart = 64 * BlackCount;
                if (redPart > 0)
                    redPart = 255;
                int greenPart = 0;

                int bluePart = 64 * WhiteCount;
                if (bluePart > 0)
                    bluePart = 255;
                
                if ((redPart > 0) && (bluePart > 0))
                {
                    redPart = 128;
                    greenPart = 64;
                    redPart = 128;
                }


                Marker.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)redPart, (byte)greenPart, (byte)bluePart));
            }
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ThreatCounter[,] heatMap = new ThreatCounter[8, 8];
        private ChessPiece _curPiece = null;

        public MainWindow()
        {
            InitializeComponent();
            BuildBoard();
        }

        private void BuildBoard()
        {
            Brush BlackBrush = new SolidColorBrush(Color.FromArgb(255,128,128,128));
            Brush WhiteBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            Brush TransparentBrush = new SolidColorBrush(Colors.Transparent);
            bool drawBlack = false;
            Rectangle newRect, heatRect;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    
                     newRect = new Rectangle();
                    newRect.Stroke = BlackBrush;
                    newRect.SnapsToDevicePixels = true;
                    if (drawBlack)
                        newRect.Fill = BlackBrush;
                    else
                        newRect.Fill = WhiteBrush;
                    drawBlack = !drawBlack;
                    Grid.SetRow(newRect, row);
                    Grid.SetColumn(newRect, col);
                    ChessBoard.Children.Add(newRect);
                    

                    heatRect = new Rectangle();
                    heatRect.Stroke = BlackBrush;
                    heatRect.SnapsToDevicePixels = true;
                    heatRect.Fill = new SolidColorBrush(Colors.Blue);
                    heatRect.IsHitTestVisible = false;
                    Grid.SetRow(heatRect, row);
                    Grid.SetColumn(heatRect, col);
                    ChessHeatMap.Children.Add(heatRect);
                    heatMap[row, col] = new ThreatCounter(heatRect);

                }
                drawBlack = !drawBlack;
            }
        }

        public ChessPiece PieceAtLocation(PieceLoc theLoc)
        {
            foreach (ChessPiece curPiece in ChessPieceLayer.Children)
            {
                if ((curPiece.Row == theLoc.Row) &&
                    (curPiece.Column == theLoc.Column))
                    return curPiece;
            }

            return null;
        }

        public ChessPiece SelectedPiece
        {
            get { return _curPiece; }
            set
            {
                if (_curPiece != null)
                    _curPiece.Selected = false;
                _curPiece = value;
                if (_curPiece != null)
                    _curPiece.Selected = true;
            }
        }

        private void ResetBoard()
        {
            CreateAndAddPiece("rook", 0, 0, true);
            CreateAndAddPiece("knight", 0, 1, true);
            CreateAndAddPiece("bishop", 0, 2, true);
            CreateAndAddPiece("queen", 0, 3, true);
            CreateAndAddPiece("king", 0, 4, true);
            CreateAndAddPiece("bishop", 0, 5, true);
            CreateAndAddPiece("knight", 0, 6, true);
            CreateAndAddPiece("rook", 0, 7, true); 

            CreateAndAddPiece("pawn", 1, 0, true);
            CreateAndAddPiece("pawn", 1, 1, true);
            CreateAndAddPiece("pawn", 1, 2, true);
            CreateAndAddPiece("pawn", 1, 3, true);
            CreateAndAddPiece("pawn", 1, 4, true);
            CreateAndAddPiece("pawn", 1, 5, true);
            CreateAndAddPiece("pawn", 1, 6, true);
            CreateAndAddPiece("pawn", 1, 7, true);


            CreateAndAddPiece("pawn", 6, 0, false);
            CreateAndAddPiece("pawn", 6, 1, false);
            CreateAndAddPiece("pawn", 6, 2, false);
            CreateAndAddPiece("pawn", 6, 3, false);
            CreateAndAddPiece("pawn", 6, 4, false);
            CreateAndAddPiece("pawn", 6, 5, false);
            CreateAndAddPiece("pawn", 6, 6, false);
            CreateAndAddPiece("pawn", 6, 7, false);


            CreateAndAddPiece("rook", 7, 0, false);
            CreateAndAddPiece("knight", 7, 1, false);
            CreateAndAddPiece("bishop", 7, 2, false);
            CreateAndAddPiece("queen", 7, 3, false);
            CreateAndAddPiece("king", 7, 4, false);
            CreateAndAddPiece("bishop", 7, 5, false);
            CreateAndAddPiece("knight", 7, 6, false);
            CreateAndAddPiece("rook", 7, 7, false);



            UpdateHeatMap();
        }

        private void CreateAndAddPiece(string pieceType, int row, int col, bool isBlack)
        {
            ChessPiece newPiece = ChessPiece.CreatePiece(pieceType, isBlack);
            newPiece.MouseDown += ChessBoard_MouseDown;
            newPiece.SetPieceLocation(row, col);
            ChessPieceLayer.Children.Add(newPiece);
        }

        private void ClearBoard()
        {
            ChessPieceLayer.Children.Clear();
            ClearHeatMap();
        }

        private void ClearHeatMap()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    heatMap[row, col].Clear();
                }
            }
        }



        private void UpdateHeatMap()
        {
            ClearHeatMap();

            foreach (ChessPiece curPiece in ChessPieceLayer.Children)
            {
                List<PieceLoc> LocList = curPiece.GetPossibleMoves();
                foreach (PieceLoc curLoc in LocList)
                {
                    if (curPiece.IsBlack)
                        heatMap[curLoc.Row, curLoc.Column].BlackCount++;
                    else
                        heatMap[curLoc.Row, curLoc.Column].WhiteCount++;
                    
                }
            }

        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetBoard();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearBoard();
        }

        private void ChessBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is ChessPiece)
            {
                if (SelectedPiece != e.OriginalSource)
                    SelectedPiece = (ChessPiece)e.OriginalSource;
                else
                    SelectedPiece = null;
            }
            else if (e.OriginalSource is Image)
            {
                Grid parent = (Grid)((Image)e.OriginalSource).Parent;
                if (SelectedPiece != (ChessPiece)parent.Parent)
                    SelectedPiece = (ChessPiece)parent.Parent;
                else
                    SelectedPiece = null;
            }
            else if ((e.OriginalSource is Rectangle) && (SelectedPiece != null))
            {
                Rectangle theSquare = (Rectangle)e.OriginalSource;
                int row = Grid.GetRow(theSquare);
                int col = Grid.GetColumn(theSquare);
                SelectedPiece.SetPieceLocation(row, col);
                UpdateHeatMap();
            }
            else
                SelectedPiece = null;

        }
    }
}
