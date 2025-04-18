using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace Figure_Builder
{
    public partial class MainWindow : Window
    {
        private List<Figures> figures = new List<Figures>();
        private delegate Figures print_fuc(int width, int height, string color);
        private int index = - 1;

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += mainWindow_closing;
            mainFrame.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(41, 41, 41));
            workPlace.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(31, 31, 31));
            colorPicker.SelectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#00FFFFFF");

            for (int i = 1; i <= 10; i++)
            {
                var rectangle = FindName($"R{i}") as System.Windows.Shapes.Rectangle;
                if (rectangle != null)
                {
                    rectangle.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(31, 31, 31));
                }
            }

            TypeComboBox.Items.Add(FigureType.Трикутник);
            TypeComboBox.Items.Add(FigureType.Чотирикутник);
            TypeComboBox.Items.Add(FigureType.Коло);

            TypeComboBox.SelectedItem = FigureType.Трикутник;
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubtypeComboBox == null)
                return;

            SubtypeComboBox.Items.Clear();

            var selectedType = TypeComboBox.SelectedItem as FigureType?;

            if (selectedType == null)
                return;

            switch (selectedType)
            {
                case FigureType.Трикутник:
                    SubtypeComboBox.Items.Add(FigureSubType.Рівносторонній);
                    SubtypeComboBox.Items.Add(FigureSubType.Рівнобедрений);
                    SubtypeComboBox.Items.Add(FigureSubType.Різносторонній);
                    SubtypeComboBox.Items.Add(FigureSubType.Прямокутний);
                    break;
                case FigureType.Чотирикутник:
                    SubtypeComboBox.Items.Add(FigureSubType.Квадрат);
                    SubtypeComboBox.Items.Add(FigureSubType.Прямокутник);
                    SubtypeComboBox.Items.Add(FigureSubType.Паралелепіпед);
                    SubtypeComboBox.Items.Add(FigureSubType.Трапеція);
                    SubtypeComboBox.Items.Add(FigureSubType.Ромб);
                    break;
                case FigureType.Коло:
                    SubtypeComboBox.Items.Add(FigureSubType.Коло);
                    SubtypeComboBox.Items.Add(FigureSubType.Овал);
                    break;
                default:
                    break;
            }
            SubtypeComboBox.SelectedIndex = 0;
        }

        // Сreate and display a new figure
        private void CreateButton_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(widthTextBox.Text) || string.IsNullOrWhiteSpace(heightTextBox.Text))
                {
                    throw new NoDataException("Будь ласка, введіть коректні значення для ширини та висоти.");
                }
                if (!int.TryParse(widthTextBox.Text, out int width) || !int.TryParse(heightTextBox.Text, out int height))
                {
                    throw new FormatException("Введіть числові значення для ширини та висоти.");
                }
                if (SubtypeComboBox.SelectedItem == null)
                {
                    throw new NoDataException("Будь ласка, виберіть підтип фігури.");
                }
                if (TypeComboBox.SelectedItem == null)
                {
                    throw new NoDataException("Будь ласка, виберіть тип фігури.");
                }
                FigureType selectedType = (FigureType)TypeComboBox.SelectedItem;
                FigureSubType selectedSubType = (FigureSubType)SubtypeComboBox.SelectedItem;

                print_fuc printFigure = find_printFunc(selectedType, selectedSubType);

                String selectedColor = colorPicker.SelectedColor.ToString();

                Figures createFigure = printFigure(width, height, selectedColor);
                index = figures.Count - 1;
                figures.Add(createFigure);

                index = figures.Count - 1;
                fillTableSquare();

            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
            catch (FormatException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Сталася невідома помилка: " + ex.Message);
            }

        }

        // Display information about the figure
        private void InfoButton_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (index < 0) { throw new InvalidOperationException("Щоб переглянути інформацію, створіть принаймні одну фігуру."); }
                Window1 infoWindow = new Window1();
                infoWindow.setInfo(figures[index].convertToArray());
                infoWindow.Owner = this;
                if (infoWindow.ShowDialog() == true)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*" };
                    bool? result = saveFileDialog.ShowDialog();
                    if (result == true)
                    {
                        string fileName = saveFileDialog.FileName;
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            System.IO.File.WriteAllText(fileName, "");
                            figures[index].writeToFile(fileName);
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }

        // Update information about the figure
        private void RefreshButton_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (figures.Count < 1)
                    throw new NoDataException("Щоб оновити дані, створіть принаймні одну фігуру");
                int width = Convert.ToInt32(widthTextBox.Text);
                int height = Convert.ToInt32(heightTextBox.Text);

                FigureType selectedType = (FigureType)TypeComboBox.SelectedItem;
                FigureSubType selectedSubType = (FigureSubType)SubtypeComboBox.SelectedItem;

                print_fuc printFigure = find_printFunc(selectedType, selectedSubType);

                String selectedColor = colorPicker.SelectedColor.ToString();

                Figures createFigure = printFigure(width, height, selectedColor);
                figures[index] = createFigure;
                updateInfoFrame();
                fillTableSquare();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        private void updateInfoFrame()
        {
            TypeComboBox.SelectedItem = figures[index].type;
            SubtypeComboBox.SelectedItem = figures[index].subType;
            widthTextBox.Text = figures[index].width.ToString();
            heightTextBox.Text = figures[index].height.ToString();
            colorPicker.SelectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(figures[index].color);
        }

        // Sort the remaining ten shapes by area
        private void SortButton_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (figures.Count < 10)
                    throw new NoDataException("Щоб посортувати дані, створіть мінімум 10 фігур");
                ShellSort(figures, 10);
                fillTableSquare();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        private void InsertionSort(List<Figures> arr)
        {
            int k = 0;
            Figures fix = arr[0];
            for (int i = 0; i < arr.Count; ++i)
            {
                fix = arr[i];
                k = i - 1;
                while (k > -1 && arr[k] > fix)
                {
                    arr[k + 1] = arr[k];
                    k--;
                }
                arr[k + 1] = fix;
            }
        }
        private void ShellSort(List<Figures> arr, int count)
        {
            int d = count / 2, count_elem = 0, elem = 0, max_index = 0, firstIndex = arr.Count - count;
            while (d > 0)
            {
                count_elem = count / d;
                max_index = count_elem * d;
                for (int i = 0; i < d; ++i)
                {
                    elem = count_elem;
                    if (max_index + i < count)
                        elem++;
                    List<Figures> temp = new List<Figures>(elem);
                    int y = 0;
                    for (int j = 0; j < elem; ++j)
                        temp.Add(arr[firstIndex + j * d + i]);
                    InsertionSort(temp);
                    for (int j = 0; j < elem; j++)
                        arr[firstIndex + j * d + i] = temp[j];
                    if (d == 1)
                        break;
                }
                d /= 2;
            }
        }

        // Delete a figure
        private void DeleteButton_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (figures.Count < 1)
                    throw new NoDataException("Немає створених фігур, щоб видалити");
                figures.RemoveAt(index);
                fillTableSquare();
                fillTableSquareAfterDelete();
                if (figures.Count > 0)
                {
                    if(figures.Count == 0)
                        index = -1;
                    else if (figures.Count > 0 && index == 0)
                        index = 0;
                    else if(index == figures.Count - 1)
                        index--;
                    if (index > -1)
                    {
                        FigureType type = figures[index].type;
                        FigureSubType subType = figures[index].subType;
                        print_fuc printFigure = find_printFunc(type, subType);
                        printFigure(figures[index].width, figures[index].height, figures[index].color);
                    }
                    updateInfoFrame();
                }
                else
                {
                    PointCollection points_Parallelepiped = new PointCollection
                    {
                        new System.Windows.Point(0, 0)
                    };
                    figure.Points = points_Parallelepiped;
                }
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }

        // Write figure data to a file
        private void WriteFileButton_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (figures.Count < 1)
                    throw new NoDataException("Щоб записати дані у фал, створітьь принаймні одну фігуру");
                WriteToFile writeWindow = new WriteToFile();
                writeWindow.Owner = this;
                if (writeWindow.ShowDialog() == true)
                {
                    int selectedOption = writeWindow.SelectedOption;
                    HandleSaveOption(selectedOption);
                }
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        private void HandleSaveOption(int selectedOption)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
            switch (selectedOption)
            {
                case 0:
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string fileName = saveFileDialog.FileName;
                        System.IO.File.WriteAllText(fileName, "");
                        figures[index].writeToFile(fileName);
                    }
                    break;
                case 1:
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string fileName = saveFileDialog.FileName;
                        System.IO.File.WriteAllText(fileName, "");
                        foreach (Figures figure in figures)
                        {
                            figure.writeToFile(fileName);
                        }
                    }
                    break;
                case 2:
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        // Create and display an equilateral triangle
        private Figures Print_Equilateral(int width, int height, string color)
        {
            double sideLen_side1 = 0;
            PointCollection points_Equilateral = new PointCollection
            {
                new System.Windows.Point(width / 2, 0),
                new System.Windows.Point(width, Math.Sqrt(3) / 2 * width),
                new System.Windows.Point(0, Math.Sqrt(3) / 2 * width)
            };
            figureGrid.Height = width;
            figureGrid.Width = width;
            figure.Points = points_Equilateral;

            sideLen_side1 = Math.Sqrt(Math.Pow((width / 2 - width), 2) + Math.Pow((0 - Math.Sqrt(3) / 2 * width), 2));
            Figures triangle_Equilateral = new Triangle_Equilateral(width, height, sideLen_side1, color);
            Brush fillBrush = (Brush)new BrushConverter().ConvertFromString(color);
            figure.Fill = fillBrush;
            return triangle_Equilateral;
        }

        // Сreate and display an isosceles triangle
        private Figures Print_Isosceles(int width, int height, string color)
        {
            double sideLen_side1 = 0, sideLen_side2 = 0, sideLen_side3 = 0;
            PointCollection points_Isosceles = new PointCollection
            {
                new System.Windows.Point(width / 2, 0),
                new System.Windows.Point(width, height),
                new System.Windows.Point(0, height)
            };
            figureGrid.Width = width;
            figureGrid.Height = height;
            figure.Points = points_Isosceles;
            sideLen_side1 = Math.Sqrt(Math.Pow((width / 2 - width), 2) + Math.Pow((0 - height), 2));
            sideLen_side2 = Math.Sqrt(Math.Pow((width - 0), 2) + Math.Pow((height - height), 2));
            sideLen_side3 = Math.Sqrt(Math.Pow((0 - width / 2), 2) + Math.Pow((height - 0), 2));
            Figures triangle_Isosceles = new Triangle_Isosceles(width, height, sideLen_side1, sideLen_side2, sideLen_side3, color);
            Brush fillBrush = (Brush)new BrushConverter().ConvertFromString(color);
            figure.Fill = fillBrush;
            return triangle_Isosceles;
        }

        // Сreate and display a diverse triangle
        private Figures Print_Diverse(int width, int height, string color)
        {
            double sideLen_side1 = 0, sideLen_side2 = 0, sideLen_side3 = 0;
            PointCollection points_Diverse = new PointCollection
            {
                new System.Windows.Point(0.1 * width, 0.1 * height),
                new System.Windows.Point(0.9 * width, 0.2 * height),
                new System.Windows.Point(0.3 * width, 0.8 * height)
            };
            figureGrid.Width = width;
            figureGrid.Height = height;
            figure.Points = points_Diverse;
            sideLen_side1 = Math.Sqrt(Math.Pow((0.1 * width - 0.9 * width), 2) + Math.Pow((0.1 * height - 0.2 * height), 2));
            sideLen_side2 = Math.Sqrt(Math.Pow((0.9 * width - 0.3 * width), 2) + Math.Pow((0.2 * height - 0.8 * height), 2));
            sideLen_side3 = Math.Sqrt(Math.Pow((0.3 * width - 0.1 * width), 2) + Math.Pow((0.8 * height - 0.1 * height), 2));
            Figures triangle_Diverse = new Triangle_Diverse(width, height, sideLen_side1, sideLen_side2, sideLen_side3, color);
            Brush fillBrush = (Brush)new BrushConverter().ConvertFromString(color);
            figure.Fill = fillBrush;
            return triangle_Diverse;
        }

        // Create and display a right triangle
        private Figures Print_Right(int width, int height, string color)
        {
            double sideLen_side1 = 0, sideLen_side2 = 0, sideLen_side3 = 0;
            PointCollection points_Right = new PointCollection
            {
                new System.Windows.Point(0, 0),
                new System.Windows.Point(width, height),
                new System.Windows.Point(0, height)
            };
            figureGrid.Width = width;
            figureGrid.Height = height;
            figure.Points = points_Right;
            sideLen_side1 = Math.Sqrt(Math.Pow((0 - width), 2) + Math.Pow((0 - height), 2));
            sideLen_side2 = Math.Sqrt(Math.Pow((width - 0), 2) + Math.Pow((height - height), 2));
            sideLen_side3 = Math.Sqrt(Math.Pow((0 - 0), 2) + Math.Pow((height - 0), 2));
            Figures triangle_Right = new Triangle_Right(width, height, sideLen_side1, sideLen_side2, sideLen_side3, color);
            Brush fillBrush = (Brush)new BrushConverter().ConvertFromString(color);
            figure.Fill = fillBrush;
            return triangle_Right;
        }

        // Сreate and display a square
        private Figures Print_Square(int width, int height, string color)
        {
            double sideLen_side1 = 0, sideLen_side2 = 0, sideLen_side3 = 0, sideLen_side4 = 0;
            PointCollection points_Square = new PointCollection
            {
                new System.Windows.Point(0, 0),
                new System.Windows.Point(width, 0),
                new System.Windows.Point(width, height),
                new System.Windows.Point(0, height)
            };
            figureGrid.Width = width;
            figureGrid.Height = height;
            figure.Points = points_Square;
            sideLen_side1 = Math.Sqrt(Math.Pow((0 - width), 2) + Math.Pow((0 - 0), 2));
            sideLen_side2 = Math.Sqrt(Math.Pow((width - width), 2) + Math.Pow((0 - height), 2));
            sideLen_side3 = Math.Sqrt(Math.Pow((width - 0), 2) + Math.Pow((height - height), 2));
            sideLen_side4 = Math.Sqrt(Math.Pow((0 - 0), 2) + Math.Pow((height - 0), 2));
            Figures rectangle_Square = new Rectangle_Square(width, height, sideLen_side1, color);
            Brush fillBrush = (Brush)new BrushConverter().ConvertFromString(color);
            figure.Fill = fillBrush;
            return rectangle_Square;
        }

        // Create and display a rectangle
        private Figures Print_Rectangle(int width, int height, string color)
        {
            double sideLen_side1 = 0, sideLen_side2 = 0, sideLen_side3 = 0, sideLen_side4 = 0;
            PointCollection points_Rectangle = new PointCollection
            {
                new System.Windows.Point(0, 0),
                new System.Windows.Point(width, 0),
                new System.Windows.Point(width, height),
                new System.Windows.Point(0, height)
            };
            figureGrid.Width = width;
            figureGrid.Height = height;
            figure.Points = points_Rectangle;
            sideLen_side1 = Math.Sqrt(Math.Pow((0 - width), 2) + Math.Pow((0 - 0), 2));
            sideLen_side2 = Math.Sqrt(Math.Pow((width - width), 2) + Math.Pow((0 - height), 2));
            sideLen_side3 = Math.Sqrt(Math.Pow((width - 0), 2) + Math.Pow((height - height), 2));
            sideLen_side4 = Math.Sqrt(Math.Pow((0 - 0), 2) + Math.Pow((height - 0), 2));
            Figures rectangle_Rectangle = new Rectangle_Rectangle(width, height, sideLen_side1, sideLen_side2, color);
            Brush fillBrush = (Brush)new BrushConverter().ConvertFromString(color);
            figure.Fill = fillBrush;
            return rectangle_Rectangle;
        }

        // Сreate and display a parallelepiped
        private Figures Print_Parallelepiped(int width, int height, string color)
        {
            double sideLen_side1 = 0, sideLen_side2 = 0, sideLen_diagonal1 = 0, sideLen_diagonal2 = 0;
            PointCollection points_Parallelepiped = new PointCollection
            {
                new System.Windows.Point(0, 0),
                new System.Windows.Point(width*0.7, 0),
                new System.Windows.Point(width, height),
                new System.Windows.Point(0.3*width, height)
            };
            figureGrid.Width = width;
            figureGrid.Height = height;
            figure.Points = points_Parallelepiped;
            sideLen_side1 = Math.Sqrt(Math.Pow((0 - width * 0.7), 2) + Math.Pow((0 - 0), 2));
            sideLen_side2 = Math.Sqrt(Math.Pow((width * 0.7 - width), 2) + Math.Pow((0 - height), 2));
            sideLen_diagonal1 = Math.Sqrt(Math.Pow((0 - width), 2) + Math.Pow((height - 0), 2));
            sideLen_diagonal2 = Math.Sqrt(Math.Pow((width * 0.7 - 0.3 * width), 2) + Math.Pow((height - 0), 2));
            Figures rectangle_Parallelepiped = new Rectangle_Parallelepiped(width, height, sideLen_side1, sideLen_side2, sideLen_diagonal1, sideLen_diagonal2, color);
            Brush fillBrush = (Brush)new BrushConverter().ConvertFromString(color);
            figure.Fill = fillBrush;
            return rectangle_Parallelepiped;
        }

        // Сreate and display a trapezoid
        private Figures Print_Trapezoid(int width, int height, string color)
        {
            double sideLen_side1 = 0, sideLen_side2 = 0, sideLen_side3 = 0;
            PointCollection points_Trapezoid = new PointCollection
            {
                new System.Windows.Point(0, height),
                new System.Windows.Point(width*0.3, 0),
                new System.Windows.Point(width*0.7, 0),
                new System.Windows.Point(width, height)
            };
            figureGrid.Width = width;
            figureGrid.Height = height;
            figure.Points = points_Trapezoid;
            sideLen_side1 = Math.Sqrt(Math.Pow((0 - width * 0.3), 2) + Math.Pow((height - 0), 2));
            sideLen_side2 = Math.Sqrt(Math.Pow((width * 0.3 - width * 0.7), 2) + Math.Pow((0 - 0), 2));
            sideLen_side3 = Math.Sqrt(Math.Pow((width - 0), 2) + Math.Pow((height - height), 2));
            Figures rectangle_Trapezoid = new Rectangle_Trapezoid(width, height, sideLen_side1, sideLen_side2, sideLen_side3, color);
            Brush fillBrush = (Brush)new BrushConverter().ConvertFromString(color);
            figure.Fill = fillBrush;
            return rectangle_Trapezoid;
        }

        // Сreate and display a rhombus
        private Figures Print_Rhombus(int width, int height, string color)
        {
            double sideLen_side1 = 0, sideLen_diagonal1 = 0, sideLen_diagonal2 = 0;
            PointCollection points_Rhombus = new PointCollection
            {
                new System.Windows.Point(width/2, 0),
                new System.Windows.Point(width, height/2),
                new System.Windows.Point(width/2, height),
                new System.Windows.Point(0, height/2)
            };
            figureGrid.Width = width;
            figureGrid.Height = height;
            figure.Points = points_Rhombus;
            sideLen_side1 = Math.Sqrt(Math.Pow((width / 2 - width), 2) + Math.Pow((0 - height / 2), 2));
            sideLen_diagonal1 = Math.Sqrt(Math.Pow((width / 2 - width / 2), 2) + Math.Pow((0 - height), 2));
            sideLen_diagonal2 = Math.Sqrt(Math.Pow((width - 0), 2) + Math.Pow((height / 2 - height / 2), 2));
            Figures rectangle_Rhombus = new Rectangle_Rhombus(width, height, sideLen_side1, sideLen_diagonal1, sideLen_diagonal2, color);
            Brush fillBrush = (Brush)new BrushConverter().ConvertFromString(color);
            figure.Fill = fillBrush;
            return rectangle_Rhombus;
        }

        // Сreate and display a circle
        private Figures Print_Circle(int width, int height, string color)
        {
            PointCollection points_Circle = new PointCollection();
            for (int i = 0; i < 100; i++)
            {
                double angle = i * 2 * Math.PI / 100;
                double x = width / 2 + width / 2 * Math.Cos(angle);
                double y = height / 2 + height / 2 * Math.Sin(angle);
                points_Circle.Add(new System.Windows.Point(x, y));
            }
            figureGrid.Width = width;
            figureGrid.Height = height;
            figure.Points = points_Circle;
            Figures Circle_circle = new Circle_Circle(width, height, width / 2, color);
            Brush fillBrush = (Brush)new BrushConverter().ConvertFromString(color);
            figure.Fill = fillBrush;
            return Circle_circle;
        }

        // Сreate and display an oval
        private Figures Print_Ellipse(int width, int height, string color)
        {
            PointCollection points_Ellipse = new PointCollection();
            for (int i = 0; i < 100; i++)
            {
                double angle = i * 2 * Math.PI / 100;
                double x = width / 2 + width / 2 * Math.Cos(angle);
                double y = height / 2 + height / 2 * Math.Sin(angle);
                points_Ellipse.Add(new System.Windows.Point(x, y));
            }
            figureGrid.Width = width;
            figureGrid.Height = height;
            figure.Points = points_Ellipse;
            Figures Circle_ellipse = new Circle_Ellipse(width, height, width / 2, height / 2, color);
            Brush fillBrush = (Brush)new BrushConverter().ConvertFromString(color);
            figure.Fill = fillBrush;
            return Circle_ellipse;
        }

        // Update the table with information about the area
        private void fillTableSquare()
        {
            int countElement = figures.Count();
            if (countElement > 10)
            {
                S10.Content = figures[countElement - 1].Area;
                S9.Content = figures[countElement - 2].Area;
                S8.Content = figures[countElement - 3].Area;
                S7.Content = figures[countElement - 4].Area;
                S6.Content = figures[countElement - 5].Area;
                S5.Content = figures[countElement - 6].Area;
                S4.Content = figures[countElement - 7].Area;
                S3.Content = figures[countElement - 8].Area;
                S2.Content = figures[countElement - 9].Area;
                S1.Content = figures[countElement - 10].Area;
            }
            else {
                for (int i = 1; i < countElement + 1; i++)
                {
                    switch (i)
                    {
                        case 1:
                            S1.Content = figures[i - 1].Area;
                            break;
                        case 2:
                            S2.Content = figures[i - 1].Area;
                            break;
                        case 3:
                            S3.Content = figures[i - 1].Area;
                            break;
                        case 4:
                            S4.Content = figures[i - 1].Area;
                            break;
                        case 5:
                            S5.Content = figures[i - 1].Area;
                            break;
                        case 6:
                            S6.Content = figures[i - 1].Area;
                            break;
                        case 7:
                            S7.Content = figures[i - 1].Area;
                            break;
                        case 8:
                            S8.Content = figures[i - 1].Area;
                            break;
                        case 9:
                            S9.Content = figures[i - 1].Area;
                            break;
                        case 10:
                            S10.Content = figures[i - 1].Area;
                            break;
                    }
                }
            }
        }

        // Update the table with information about the area
        private void fillTableSquareAfterDelete()
        {
            int countElement = figures.Count(), i = 0;
            if (countElement < 10)
            {
                switch (countElement)
                {
                    case 0:
                        S1.Content = "?";
                        break;
                    case 1:
                        S2.Content = "?";
                        break;
                    case 2:
                        S3.Content = "?";
                        break;
                    case 3:
                        S4.Content = "?";
                        break;
                    case 4:
                        S5.Content = "?";
                        break;
                    case 5:
                        S6.Content = "?";
                        break;
                    case 6:
                        S7.Content = "?";
                        break;
                    case 7:
                        S8.Content = "?";
                        break;
                    case 8:
                        S9.Content = "?";
                        break;
                    case 9:
                        S10.Content = "?";
                        break;
                }
            }
        }

        // Linking text boxes for correct data entry
        private bool isUpdating = false;
        private void WidthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                IsTextNumeric(widthTextBox.Text);

                if (widthTextBox.Text.Length > 3)
                {
                    widthTextBox.Text = widthTextBox.Text.Substring(0, 3);
                    widthTextBox.CaretIndex = widthTextBox.Text.Length;
                }
                if (int.TryParse(widthTextBox.Text, out int value))
                {
                    if (value > 500)
                    {
                        widthTextBox.Text = "500";
                        widthTextBox.CaretIndex = widthTextBox.Text.Length;
                    }
                }

                var selectedSubType = SubtypeComboBox.SelectedItem as FigureSubType?;
                if (isUpdating || (selectedSubType != FigureSubType.Рівносторонній && selectedSubType != FigureSubType.Квадрат && selectedSubType != FigureSubType.Коло))
                    return;

                isUpdating = true;
                heightTextBox.Text = widthTextBox.Text;
                isUpdating = false;
            }
            catch (InvalidInputException)
            {
                widthTextBox.Clear();
            }
        }

        private void UpdateInputs(object sender, SelectionChangedEventArgs e)
        {
            try {
                var selectedSubType = SubtypeComboBox.SelectedItem as FigureSubType?;
                if (isUpdating || (selectedSubType != FigureSubType.Рівносторонній && selectedSubType != FigureSubType.Квадрат && selectedSubType != FigureSubType.Коло))
                    return;

                heightTextBox.Text = widthTextBox.Text;
                widthTextBox.Text = heightTextBox.Text;
            }
            catch
            {
                widthTextBox.Clear();
            }
        }

        private void HeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                IsTextNumeric(heightTextBox.Text);

                if (heightTextBox.Text.Length > 3)
                {
                    heightTextBox.Text = heightTextBox.Text.Substring(0, 3);
                    heightTextBox.CaretIndex = heightTextBox.Text.Length;
                }
                if (int.TryParse(heightTextBox.Text, out int value))
                {
                    if (value > 500)
                    {
                        heightTextBox.Text = "500";
                        heightTextBox.CaretIndex = heightTextBox.Text.Length;
                    }
                }

                var selectedSubType = SubtypeComboBox.SelectedItem as FigureSubType?;
                if (isUpdating || (selectedSubType != FigureSubType.Рівносторонній && selectedSubType != FigureSubType.Квадрат && selectedSubType != FigureSubType.Коло))
                    return;

                isUpdating = true;
                widthTextBox.Text = heightTextBox.Text;
                isUpdating = false;
            }
            catch (InvalidInputException)
            {
                heightTextBox.Clear();
            }
        }
        private bool IsTextNumeric(string text)
        {
            bool rez = text.All(char.IsDigit);
            if (!rez)
                throw new InvalidInputException();
            return rez;
        }

        private void HeightTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Дозволяємо тільки цифри
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true; // Блокуємо введення нецифрових символів
            }
        }

        // Select a function to display a figure
        private print_fuc find_printFunc(FigureType type, FigureSubType subType)
        {
            switch (type)
            {
                case FigureType.Трикутник:
                    switch (subType)
                    {
                        case FigureSubType.Рівносторонній:
                            return new print_fuc(Print_Equilateral);
                        case FigureSubType.Рівнобедрений:
                            return new print_fuc(Print_Isosceles);
                        case FigureSubType.Різносторонній:
                            return new print_fuc(Print_Diverse);
                        case FigureSubType.Прямокутний:
                            return new print_fuc(Print_Right);
                    }
                    break;
                case FigureType.Чотирикутник:
                    switch (subType)
                    {
                        case FigureSubType.Квадрат:
                            return new print_fuc(Print_Square);
                        case FigureSubType.Прямокутник:
                            return new print_fuc(Print_Rectangle);
                        case FigureSubType.Паралелепіпед:
                            return new print_fuc(Print_Parallelepiped);
                        case FigureSubType.Трапеція:
                            return new print_fuc(Print_Trapezoid);
                        case FigureSubType.Ромб:
                            return new print_fuc(Print_Rhombus);
                    }
                    break;
                case FigureType.Коло:
                    switch (subType)
                    {
                        case FigureSubType.Коло:
                            return new print_fuc(Print_Circle);
                        case FigureSubType.Овал:
                            return new print_fuc(Print_Ellipse);
                    }
                    break;
            }
            return null;
        }

        // Display the selected figure
        private void R1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (S1.Content.ToString() == "?")
                    throw new NoDataException("Немає інформації. Фігуру ще не створено");
                index = figures.Count();
                if (index > 10)
                    index -= 10;
                else
                    index = 0;
                FigureType type = figures[index].type;
                FigureSubType subType = figures[index].subType;
                print_fuc printFigure = find_printFunc(type, subType);
                printFigure(figures[index].width, figures[index].height, figures[index].color);
                updateInfoFrame();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        // Display the selected figure
        private void R2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (S2.Content.ToString() == "?")
                    throw new NoDataException("Немає інформації. Фігуру ще не створено");
                index = figures.Count();
                if (index > 10)
                    index -= 9;
                else
                    index = 1;
                FigureType type = figures[index].type;
                FigureSubType subType = figures[index].subType;
                print_fuc printFigure = find_printFunc(type, subType);
                printFigure(figures[index].width, figures[index].height, figures[index].color);
                updateInfoFrame();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        // Display the selected figure
        private void R3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (S3.Content.ToString() == "?")
                    throw new NoDataException("Немає інформації. Фігуру ще не створено");
                index = figures.Count();
                if (index > 10)
                    index -= 8;
                else
                    index = 2;
                FigureType type = figures[index].type;
                FigureSubType subType = figures[index].subType;
                print_fuc printFigure = find_printFunc(type, subType);
                printFigure(figures[index].width, figures[index].height, figures[index].color);
                updateInfoFrame();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        // Display the selected figure
        private void R4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (S4.Content.ToString() == "?")
                    throw new NoDataException("Немає інформації. Фігуру ще не створено");
                index = figures.Count();
                if (index > 10)
                    index -= 7;
                else
                    index = 3;
                FigureType type = figures[index].type;
                FigureSubType subType = figures[index].subType;
                print_fuc printFigure = find_printFunc(type, subType);
                printFigure(figures[index].width, figures[index].height, figures[index].color);
                updateInfoFrame();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        // Display the selected figure
        private void R5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (S5.Content.ToString() == "?")
                    throw new NoDataException("Немає інформації. Фігуру ще не створено");
                index = figures.Count();
                if (index > 10)
                    index -= 6;
                else
                    index = 4;
                FigureType type = figures[index].type;
                FigureSubType subType = figures[index].subType;
                print_fuc printFigure = find_printFunc(type, subType);
                printFigure(figures[index].width, figures[index].height, figures[index].color);
                updateInfoFrame();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        // Display the selected figure
        private void R6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (S6.Content.ToString() == "?")
                    throw new NoDataException("Немає інформації. Фігуру ще не створено");
                index = figures.Count();
                if (index > 10)
                    index -= 5;
                else
                    index = 5;
                FigureType type = figures[index].type;
                FigureSubType subType = figures[index].subType;
                print_fuc printFigure = find_printFunc(type, subType);
                printFigure(figures[index].width, figures[index].height, figures[index].color);
                updateInfoFrame();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        // Display the selected figure
        private void R7_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (S7.Content.ToString() == "?")
                    throw new NoDataException("Немає інформації. Фігуру ще не створено");
                index = figures.Count();
                if (index > 10)
                    index -= 4;
                else
                    index = 6;
                FigureType type = figures[index].type;
                FigureSubType subType = figures[index].subType;
                print_fuc printFigure = find_printFunc(type, subType);
                printFigure(figures[index].width, figures[index].height, figures[index].color);
                updateInfoFrame();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        // Display the selected figure
        private void R8_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (S8.Content.ToString() == "?")
                    throw new NoDataException("Немає інформації. Фігуру ще не створено");
                index = figures.Count();
                if (index > 10)
                    index -= 3;
                else
                    index = 7;
                FigureType type = figures[index].type;
                FigureSubType subType = figures[index].subType;
                print_fuc printFigure = find_printFunc(type, subType);
                printFigure(figures[index].width, figures[index].height, figures[index].color);
                updateInfoFrame();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        // Display the selected figure
        private void R9_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (S9.Content.ToString() == "?")
                    throw new NoDataException("Немає інформації. Фігуру ще не створено");
                index = figures.Count();
                if (index > 10)
                    index -= 2;
                else
                    index = 8;
                FigureType type = figures[index].type;
                FigureSubType subType = figures[index].subType;
                print_fuc printFigure = find_printFunc(type, subType);
                printFigure(figures[index].width, figures[index].height, figures[index].color);
                updateInfoFrame();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        // Display the selected figure
        private void R10_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (S10.Content.ToString() == "?")
                    throw new NoDataException("Немає інформації. Фігуру ще не створено");
                index = figures.Count();
                if (index > 10)
                    index -= 1;
                else
                    index = 9;
                FigureType type = figures[index].type;
                FigureSubType subType = figures[index].subType;
                print_fuc printFigure = find_printFunc(type, subType);
                printFigure(figures[index].width, figures[index].height, figures[index].color);
                updateInfoFrame();
            }
            catch (NoDataException ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }

        // End of the program
        private void mainWindow_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            if (figures.Count < 1)
            {
                e.Cancel= false;
                return;
            }
                
            CloseWindow closeWindow = new CloseWindow();
            closeWindow.Owner = this;
            if (closeWindow.ShowDialog() == true)
            {
                int selectedOption = closeWindow.SelectedOption;
                HandleCloseOption(selectedOption, e);
            }
        }
        private void HandleCloseOption(int selectedOption, System.ComponentModel.CancelEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
            switch (selectedOption)
            {
                case 0:
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string fileName = saveFileDialog.FileName;
                        System.IO.File.WriteAllText(fileName, "");
                        foreach (Figures figure in figures)
                        {
                            figure.writeToFile(fileName);
                        }
                    }
                    e.Cancel = false;
                    break;
                case 1:
                    e.Cancel = false;
                    break;
                case 2:
                    e.Cancel = true;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
