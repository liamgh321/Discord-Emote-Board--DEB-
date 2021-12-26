using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;

namespace DiscordEmoji
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Populate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

        }

        private void CopyImgToClipboard(string path)
        {
            Clipboard.Clear();
            //string path = "C:\\Users\\liamg\\Dropbox\\Visual Studio Projects\\2021\\DiscordEmoji\\DiscordEmoji\\bin\\Debug\\Emotes\\Anime\\peepocry.png";
            Clipboard.SetImage(Image.FromFile(path)); //CURRENT WORKING CODE
            //Clipboard.SetData(byte, System.IO.File.ReadAllBytes(path));
            //Clipboard.SetData()
            //StringCollection filePath = new StringCollection();
            //List<string> filePath = new List<string>();
            /*filePath.Add(path);
            filePath.Add(path);
            Clipboard.SetFileDropList(filePath);*/
        }

        private void TestMethod(object sender, EventArgs e, int x)
        {
            Console.WriteLine(x);
        }

        private void Populate()
        {
            int xPos = 20; // initial xpos for section label creation
            int yPos = 50; // initial ypos for section label creation
            int xPosBtn; // will hold the xPos value for the button being created
            int yPosBtn; // will hold the yPos valye for the button being created
            int winWidth = 613;
            int winHeight = 438;
            int btnWidth = 75;
            int btnHeight = 75;
            int maxRowBtn; // will hold value for the maximum # of buttons per row
            int sectionHeight; // Will hold the value that determines the height of the section between each label.
            int imageIndex; // used to point to the right image to make a button for
            double numRows; // will hold value for the maximum number of rows in the section.
            string[] folders; // holds the files paths of all the folders in the directory, currently Debug folder in project bin.
            int folderCount = System.IO.Directory.GetDirectories(@"Emotes").Length; //gets the number of folders
            folders = System.IO.Directory.GetDirectories(@"Emotes"); //fills the above string[] folders variable
            string[] imagePaths; // similar to string[] folders, but for the images in the folders
            string[] imageNames; // similar to imagePaths but will containt the name of the image file only, the rest of the path will be trimmed.
            double imageCount; // will hold the number of images in the folder

            for (int i = 0; i < folders.Length; i++) //removes the "emotes\" part of the file paths
            {
                folders[i] = folders[i].Remove(0, 7);
            }

            for (int i = 0; i < folderCount; i++) // creates a label for each folder name that the buttons are going to go under.
            {
                // create label
                var label = new Label
                {
                    Tag = string.Format("Lbl{0}", i),
                    Text = string.Format("{0}", folders[i]),
                    Location = new Point(xPos, yPos)
                };
                Controls.Add(label); // The previous 6 lines and this create a new label w/ the tag lbl(# label it is in created order),
                                     // text set to the folder names, and the position set by xPos and yPos
                                     //yPos = yPos + 10; //increases yPos by 100 so the labels are set apart for now
                                     // NOTE: the distance between labels should be based on how long the distance between each section will need to be.

                imagePaths = System.IO.Directory.GetFiles(@"Emotes\" + folders[i]);
                imageNames = System.IO.Directory.GetFiles(@"Emotes\" + folders[i]); // Did not use imageNames = imagePaths because the following changes to imageNames
                                                                                    // would also change imagePaths.
                imageCount = imagePaths.Length;

                for (int j = 0; j < imageNames.Length; j++) //removes the "emotes\ + [Folder Name]\" part of the file paths
                {
                    int trimmer = 8 + folders[i].Length;
                    imageNames[j] = imageNames[j].Remove(0, trimmer);
                }

                if (imagePaths.Length == 0) //placeholder, deals with an empty folder.
                {
                    System.Windows.Forms.MessageBox.Show("imagePaths.length = " + imagePaths.Length.ToString());
                    continue;
                }

                maxRowBtn = (winWidth - 40) / (btnWidth + 6); // 40 is the sum of the left margin, and the desired right margin, both 20
                                                              // 6 is the desired spacing between each button, in this case we are treating
                                                              // the space each button takes up as the width of the button plus the width of the
                                                              // space to the right of the button
                                                              //Console.WriteLine(maxRowBtn); //DEBUG

                numRows = (double)(imageCount / maxRowBtn); // determines the number of rows needed based off of the # of btns that can fit per row.
                numRows = Math.Ceiling(numRows); // Then that number is rounded up to the nearest integer to ensure there is a row for the last < maxRowBtn row of btns.

                sectionHeight = (int)((btnHeight + 6) * numRows); // Section height determined by the height of the buttons * the desired margin of 6x,
                                                                  // times the number of rows.
                xPosBtn = xPos;
                yPosBtn = yPos + label.Height;

                imageIndex = 0;
                for (int j = 0; j < numRows; j++) // this for loop loops us through the actions per each row of btns, 
                                                  // ie fully populate a row then move on to the next one, loops the amount of times there will be rows.
                {
                    if ((imageCount - imageIndex) >= maxRowBtn) //this if statement holds the for loop to fill each row besides the last one
                    {
                        for (int k = 0; k < maxRowBtn && k < imageCount; k++)
                        {
                            var button = new Button { };
                            button.Text = imageNames[imageIndex];
                            button.Height = btnHeight;
                            button.Width = btnWidth;
                            button.Location = new Point(xPosBtn, yPosBtn);

                            int paramIndex = imageIndex; // Put value of imageIndex into paramIndex because imageIndex will most likely change value
                                                         // and we want to use the value of imageIndex at the moment of this button creation.
                            button.Click += (s, e) => { CopyImgToClipboard(imagePaths[paramIndex]); }; // Using lambda noation, we create an anonymous event
                                                                                                       // handler that runs CopyImgToClipboard with the file path
                                                                                                       // for the correct image as the parameter

                            Controls.Add(button); //adds the button

                            xPosBtn = xPosBtn + btnWidth + 6; //increases the xpostion for the next btn by the width of a button + margin (6 = margin)
                            imageIndex++;
                        }
                    }
                    else //this if statement holds the for loop to fill the final row of a section.
                    {
                        double stopper = imageCount - imageIndex; //this is how the for loop for the final row knows when to stop.
                        for (int k = 0; k < stopper; k++)
                        {
                            var button = new Button { };
                            button.Text = imageNames[imageIndex];
                            button.Height = btnHeight;
                            button.Width = btnWidth;
                            button.Location = new Point(xPosBtn, yPosBtn);

                            int paramIndex = imageIndex; // Put value of imageIndex into paramIndex because imageIndex will most likely change value
                                                         // and we want to use the value of imageIndex at the moment of this button's creation, but later.
                            button.Click += (s, e) => {
                                Console.WriteLine("HERE!!! Index is: " + paramIndex + ", imagePaths length is: " + imagePaths.Length);
                                CopyImgToClipboard(imagePaths[paramIndex]); }; // Using lambda notation, we create an anonymous event
                                                                               // handler that runs CopyImgToClipboard with the file path
                                                                               // for the correct image as the parameter  

                            Controls.Add(button);

                            xPosBtn = xPosBtn + btnWidth + 6; //increases the xpostion for the next btn by the width of a button + margin (6 = margin)
                            imageIndex++;
                        }
                    }
                    xPosBtn = xPos;
                    yPosBtn = yPosBtn + btnHeight + 6; //increases the ypostion for the next btn by the height of a button + margin (6 = margin)
                }

                yPos = yPos + sectionHeight + label.Height;

                Console.WriteLine("--------HERE-------- imagePaths.length: " + imagePaths.Length); // DEBUG
            }
        }
    }
}
