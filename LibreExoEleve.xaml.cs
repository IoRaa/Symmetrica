﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Projet2Cp.Utili;

namespace Projet2Cp
{


    public partial class LibreExoEleve : Page
    {/*


        Point centrePoly = new Point();

        public static Line axeSym { get; set; } // l'axe de symetrie selectionné
        public static Ellipse centreSym { get; set; } // le centre de symetrie 



        public static bool teacherMode { get; set; } //  tells wether we're in the teacher mode  or not 
        public static bool exoMode { get; set; } //  tells wether we're in the exercices mode  or not 

        private List<ShapePair> shapePairs; //represents the pairs of shapes
        private Point mousePosition; //mouse position in canvas
        private Point actualPoint;//actual point to draw in 
        private PointCollection drawingPoints;
        private double step = 25; // the step of the grid 
        private PointCollection shapepc, shapeSym; // the current shape being drawn  , and a holder for his symetrix 

        private ShapePair currentShapePair;
        private Polygon polygone;
        private Polyline polyline;
        private Ellipse ellipse;// to represent a point in the canvas
        public Line line;
        public Nullable<Point> centresym;
        TextBlock tb;


        List<dessinExo> dessinsModeExo;



        private bool isDrawing, isRotating = false;
        private bool isOverShape = false;
        private bool isTransforming = false;
        private bool isOrigin = true;
        private bool isEditing = false;


        Point clickPosition = new Point(); //always indicates where the mouse "left-clicked" on the canvas


        public static Brush trace;
        public static Brush rempli;

        public LibreExoEleve()
        {
            InitializeComponent();

            

 
            niv.b1.Click += Niv_Click;
            niv.b2.Click += Niv_Click;
            niv.b3.Click += Niv_Click;
            niv.b4.Click += Niv_Click;
            niv.b5.Click += Niv_Click;
            niv.b6.Click += Niv_Click;
            niv.b7.Click += Niv_Click;
            niv.b8.Click += Niv_Click;
            niv.b9.Click += Niv_Click;

            toolBarExo.effacerTout.Click += effacerTout;
            toolBarExo.valider.Click += valider_Click;

            canvas.MouseLeave += ellipseCleaner;
            canvas.MouseMove += currentMousePosition;
            canvas.MouseMove += translating;
            canvas.MouseMove += mouseCursor;

            initCanvas();
            dessinsModeExo = chargerDessins(@".\shapesExo.txt");
            //initialiser le centre de symetrie 
            centreSym = new Ellipse()
            {
                Height = 10,
                Width = 10,
                Fill = Brushes.Red,
                Stroke = trace,
            };


            isDrawing = true; //for now the only option available

            //canvas.MouseMove += rotating;
            // canvas.MouseLeftButtonDown += hitShape;




        }



        private void GenSym_Click(object sender, RoutedEventArgs e)
        {
            //  if(currentShapePair != null) currentShapePair.symGen(shapeMouseEnter, shapeMouseLeave);

        }




        //=======================================================================================================//
        // CurrentMousePosition : continuously updates "mousePosition" in canvas while not clicking on anything  //
        //=======================================================================================================//

        void currentMousePosition(object sender, MouseEventArgs e)
        {

            if (e.LeftButton != MouseButtonState.Pressed)
                mousePosition = e.GetPosition(canvas);
        }

        //=======================================================================================================//
        //                                          INIT_CANVAS
        //=======================================================================================================//

        private void initCanvas()
        {
            shapePairs = new List<ShapePair>();
            currentShapePair = null;
            shapepc = new PointCollection();

            polyline = new Polyline()
            {
                Stroke = trace,
                StrokeThickness = 1,
                Points = shapepc,
            };

            canvas.Children.Add(polyline);

            line = new Line()
            {
                Stroke = trace,
                StrokeThickness = 1,
            };

            canvas.Children.Add(line);
         

        }



        //====================================================================================================================//
        //  TRANSLATING: if we are drawing + leftButton pressed + we are on a shape, we translate the shape or its corner     //  
        //====================================================================================================================//



        private void translating(Object sender, MouseEventArgs e)
        {
            if (isTransforming && isDrawing && (e.LeftButton == MouseButtonState.Pressed))
            {
                Vector vec = e.GetPosition(canvas) - mousePosition;
                if (currentShapePair != null)
                    currentShapePair.translating(vec, isOrigin, mousePosition, isAxial, sline, spoint);
                mousePosition = e.GetPosition(canvas); //on reactulise clickPosition pour le prochain move
            }


        }


        //====================================================================================================================//
        //                          ELLIPSE_CLEANER : ENLEVE LA PREVIEW ELLIPSE UNE FOIS QU'ON QUITTE LE CANVAS               //  
        //====================================================================================================================//


        private void ellipseCleaner(Object sender, MouseEventArgs e)
        {
            canvas.Children.Remove(ellipse);
        }

        //====================================================================================================================//
        //                          CANVAS_MLBD : COORDINE LE TOUS !! (DETERMINE CE QU'IL FAUT FAIRE AU CLICK)                //  
        //====================================================================================================================//

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickPosition = e.GetPosition(canvas); //On recupere la position du click a toute fin utiles ...
            if (polyline != null) polyline.Stroke = trace;                                       //
            if (isDrawing)
            {
                if (!isOverShape)
                    polyline.Points.Add(actualPoint);
                else if (e.Source is Shape)
                {
                    isTransforming = true;
                    bool stop = false;
                    int i;
                    for (i = 0; i < shapePairs.Count && !stop; i++)
                    {
                        stop = (shapePairs[i].origin == e.Source);
                        for (int j = 0; j < shapePairs[i].oEllipse.Count && !stop; j++)
                            stop = (shapePairs[i].oEllipse[j] == e.Source);
                        if (stop) isOrigin = true;
                        else
                        {
                            stop = (shapePairs[i].sym == e.Source);
                            for (int j = 0; j < shapePairs[i].sEllipse.Count && !stop; j++)
                                stop = (shapePairs[i].sEllipse[j] == e.Source);
                            if (stop) isOrigin = false; //hiya normalement bla ma endir if mais ma3lih ....
                        }

                    }

                    if (stop) //hiya nrmlm bla ma nverifyi ...
                    {
                        i--;
                        currentShapePair = shapePairs[i];

                    }
                }

            }
        }

        //====================================================================================================================//
        //                          CANVAS_MLBU : COORDINE LE TOUS !! (DETERMINE CE QU'IL FAUT FAIRE AU DE_CLICK)             //  
        //====================================================================================================================//

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isTransforming = false;
        }

        //====================================================================================================================//
        //                CANVAS_MOUSE_MOVE : DESSINE LA PREVIEW ELLIPSE SI : DRAWING + NOT_OVER_SHAPE + NOT_TRANSOFRMING     //  
        //====================================================================================================================//



        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && !isOverShape && !isTransforming)
            {
                canvas.Children.Remove(ellipse);
                mousePosition = e.GetPosition(canvas);

                actualPoint.X = Math.Round(mousePosition.X / step) * step;
                actualPoint.Y = Math.Round(mousePosition.Y / step) * step;

                if (Math.Abs(actualPoint.X - mousePosition.X) < 12.5 && Math.Abs(actualPoint.Y - mousePosition.Y) < 12.5 && canvas.IsMouseOver)
                {// if the cursor is close enough to an intersection then we draw a point 
                    ellipse = new Ellipse

                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Black,
                    };
                    ellipse.MouseLeftButtonDown += canvas_MouseLeftButtonDown;
                    ellipse.MouseRightButtonDown += canvas_MouseRightButtonDown;// C'EST PROVISOIR !!
                    canvas.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, actualPoint.X - 5);
                    Canvas.SetTop(ellipse, actualPoint.Y - 5);
                }

                //---------------------------------------------------------------------------------------//

                if (polyline.Points.Count > 0 && canvas.IsMouseOver) // ie il a engagé un dessin
                {
                    line.Stroke = trace;
                    line.X1 = polyline.Points[polyline.Points.Count - 1].X;
                    line.Y1 = polyline.Points[polyline.Points.Count - 1].Y;
                    line.X2 = actualPoint.X;
                    line.Y2 = actualPoint.Y;

                }
            }

        }

        //====================================================================================================================//
        // SHAPE_MOUSE_ENTER : NETTOIE LA PREVIEW ELLIPSE ET LA BLOCK (AVEC SHAP_OVER = TRUE) DES QUE SHAPE_ENTER             //  
        //====================================================================================================================//

        public void shapeMouseEnter(object sender, MouseEventArgs e) //maybe its bad practice ....
        {
            if (isOverShape == false)
            {
                isOverShape = true;
                canvas.Children.Remove(ellipse);
            }
        }

        //====================================================================================================================//
        //                  SHAPE_MOUSE_LEAVE: DEBLOCK PREVIEW ELLIPSE (AVEC SHAPE_OVER = FALSE)                              //
        //====================================================================================================================//
        public void shapeMouseLeave(object sender, MouseEventArgs e)
        {
            isOverShape = false;
        }

        //==================================================================================================================//
        //       CANVAS_MRBD: DESSINE LE SHAPE CONTENU DANS POLYLINE                                                        //
        //==================================================================================================================//

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            // creating the shape 
            if (shapepc.Count > 1) //if the PointCollection of the polyline contains at least two points
            {
                if (Point.Equals(polyline.Points[0], polyline.Points[polyline.Points.Count - 1])) //if the shape is closed
                {
                    polygone = new Polygon()
                    {
                        Points = shapepc,
                        Fill = rempli,
                        Stroke = trace,
                        StrokeThickness = 3,
                    };

                    polygone.MouseEnter += shapeMouseEnter;
                    polygone.MouseLeave += shapeMouseLeave;
                    canvas.Children.Remove(polyline); //we remove the preview polyline
                    shapePairs.Add(new ShapePair(polygone, canvas, axeSym, shapeMouseEnter, shapeMouseLeave)); //we create a new shapePair with origin = polygon "linked to canvas"
                }

                else
                {
                    //we keep the polyline
                    polyline.StrokeThickness = 8;
                    polyline.MouseMove += shapeMouseEnter;
                    polyline.MouseLeave += shapeMouseLeave;
                    shapePairs.Add(new ShapePair(polyline, canvas, axeSym, shapeMouseEnter, shapeMouseLeave));
                }
            }
            cleaningTheMess();

        }


        private void cleaningTheMess()
        {
            //We clean the mess

            shapepc = new PointCollection();

            polyline = new Polyline()
            {
                Stroke = trace,
                StrokeThickness = 1,
                Points = shapepc,
            };

            canvas.Children.Add(polyline);


            canvas.Children.Remove(line); //we remove the last line from the canvas

            line = new Line()
            {
                Stroke = trace,
                StrokeThickness = 1,
            };

            canvas.Children.Add(line); //we add the new empty line...GarbageCollector use 100 ! XDDD


        }


        //==================================================================================================================//
        //       MOUSE_CURSOR: UPDATES THE CURSOR STYLE (AT EACH MOUSE_MOVE) DEPENDING ON THE SITUATION)                     //
        //==================================================================================================================//
        private void mouseCursor(Object sender, MouseEventArgs e)
        {
            if (isRotating)
            {

                if (e.Source is Shape) this.Cursor = Cursors.Hand;
                else this.Cursor = Cursors.Arrow;
            }
            if (isDrawing)
            {
                if (isOverShape)
                    this.Cursor = Cursors.Hand;
                else
                    this.Cursor = Cursors.Pen;
            }
        }




















        private void effacerTout(object sender, RoutedEventArgs e)
        {
            clear();
            if (isEditing)
            {
                canvas.Children.Add(tb);
                Canvas.SetTop(tb, 0);
            }


        }
        private void clear()
        {
            canvas.Children.Clear();
            initCanvas();
        }

        




        

        //==================================================================================================//
        //                                   Methodes  du mode Eleve                                        //
        //==================================================================================================//
        private void valider_Click(Object sender, RoutedEventArgs e)
        {


        }
        private void Niv_Click(object sender, RoutedEventArgs e)
        {
            dessinerDessinNum(niv.Selected());
        }
        private void dessinerDessinNum(int i)
        {
            clear();
            dessinExo poly = dessinsModeExo[i];
            selectAxe(poly.repere);
            if (poly.type)
            {
                polygone = (Polygon)poly.shape;
                polygone.MouseEnter += shapeMouseEnter;
                polygone.MouseEnter += shapeMouseEnter;
                polygone.MouseLeave += shapeMouseLeave;
                shapePairs.Add(new ShapePair(polygone, canvas, axeSym, shapeMouseEnter, shapeMouseLeave));
            }
            else
            {
                polyline = (Polyline)poly.shape;
                polyline.MouseEnter += shapeMouseEnter;
                polyline.MouseLeave += shapeMouseLeave;
                canvas.Children.Add(polyline);
                shapePairs.Add(new ShapePair(polyline, canvas, axeSym, shapeMouseEnter, shapeMouseLeave));
                cleaningTheMess();
            }

        }
        private void selectAxe(String axe)
        {

            canvas.Children.Remove(centreSym);
            centresym = null;

            axeSym = new Line();
            axeSym.StrokeThickness = 1;
            axeSym.Stroke = trace;
            canvas.Children.Add(axeSym);
            axeSym.Stroke = Brushes.Black;

            switch (axe)
            {
                case "horiz":
                    {
                        axeSym.X1 = canvas.ActualWidth / 2;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = canvas.ActualWidth / 2;
                        axeSym.Y2 = canvas.ActualHeight;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        break;
                    }
                case "verti":
                    {
                        axeSym.X1 = 0.5;
                        axeSym.Y1 = canvas.ActualHeight / 2;
                        axeSym.X2 = canvas.ActualWidth;
                        axeSym.Y2 = canvas.ActualHeight / 2;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);

                        break;
                    }
                case "diag1":
                    {
                        axeSym.X1 = 0.5;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = canvas.ActualWidth;
                        axeSym.Y2 = canvas.ActualHeight;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);

                        break;
                    }
                case "diag2":
                    {
                        axeSym.X1 = canvas.ActualWidth;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = 0.5;
                        axeSym.Y2 = canvas.ActualHeight; ;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);

                        break;
                    }
                case "centre":
                    {
                        centresym = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);
                        canvas.Children.Remove(axeSym);
                        if (!(canvas.Children.Contains(centreSym)))
                        {
                            canvas.Children.Add(centreSym);
                            Canvas.SetLeft(centreSym, canvas.ActualWidth / 2 - 5);
                            Canvas.SetTop(centreSym, canvas.ActualHeight / 2 - 5);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }




        */
    }
}
 