using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;

namespace GameCaro_SettingInforPlayer
{
    public partial class ImageEditForm : System.Windows.Forms.Form
    {
        private Image originalImage;
        private Image displayedImage;
        private float zoomFactor = 1.0f;
        private float rotationAngle = 0f;
        private bool isDraggingCrop = false;
        private Point lastMousePosition;
        private Rectangle cropArea;

        public Image CroppedImage { get; private set; }

        private Panel panelPictureBox;
        private PictureBox pictureBoxEdit;
        private TrackBar trackBarZoom;
        private Button btnRotate;
        private Button btnOK;
        private Button btnCancel;

        public ImageEditForm(Image image)
        {
            InitializeComponent();
            originalImage = image ?? throw new ArgumentNullException(nameof(image));
            displayedImage = new Bitmap(originalImage);
            cropArea = new Rectangle(0, 0, 100, 100); // Initial crop area

            // Setup PictureBox
            pictureBoxEdit.SizeMode = PictureBoxSizeMode.Normal;
            pictureBoxEdit.Image = displayedImage;
            pictureBoxEdit.MouseDown += PictureBoxEdit_MouseDown;
            pictureBoxEdit.MouseMove += PictureBoxEdit_MouseMove;
            pictureBoxEdit.MouseUp += PictureBoxEdit_MouseUp;
            pictureBoxEdit.Paint += PictureBoxEdit_Paint;

            // Setup TrackBar Zoom
            trackBarZoom.Minimum = 1;
            trackBarZoom.Maximum = 100;
            trackBarZoom.TickFrequency = 1;
            trackBarZoom.Value = 20;
            trackBarZoom.Scroll += TrackBarZoom_Scroll;

            // Setup buttons
            btnRotate.Click += BtnRotate_Click;
            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;

            UpdateImage();
        }

        private void InitializeComponent()
        {
            this.panelPictureBox = new System.Windows.Forms.Panel();
            this.pictureBoxEdit = new System.Windows.Forms.PictureBox();
            this.trackBarZoom = new System.Windows.Forms.TrackBar();
            this.btnRotate = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelPictureBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // panelPictureBox
            // 
            this.panelPictureBox.AutoScroll = true;
            this.panelPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPictureBox.Controls.Add(this.pictureBoxEdit);
            this.panelPictureBox.Location = new System.Drawing.Point(12, 12);
            this.panelPictureBox.Name = "panelPictureBox";
            this.panelPictureBox.Size = new System.Drawing.Size(431, 416);
            this.panelPictureBox.TabIndex = 0;
            // 
            // pictureBoxEdit
            // 
            this.pictureBoxEdit.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxEdit.Name = "pictureBoxEdit";
            this.pictureBoxEdit.Size = new System.Drawing.Size(431, 416);
            this.pictureBoxEdit.TabIndex = 0;
            this.pictureBoxEdit.TabStop = false;
            // 
            // trackBarZoom
            // 
            this.trackBarZoom.Location = new System.Drawing.Point(12, 434);
            this.trackBarZoom.Name = "trackBarZoom";
            this.trackBarZoom.Size = new System.Drawing.Size(431, 56);
            this.trackBarZoom.TabIndex = 1;
            // 
            // btnRotate
            // 
            this.btnRotate.Location = new System.Drawing.Point(159, 480);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(75, 30);
            this.btnRotate.TabIndex = 2;
            this.btnRotate.Text = "Rotate 90°";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(365, 480);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(262, 480);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            // 
            // ImageEditForm
            // 
            this.ClientSize = new System.Drawing.Size(458, 520);
            this.Controls.Add(this.panelPictureBox);
            this.Controls.Add(this.trackBarZoom);
            this.Controls.Add(this.btnRotate);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImageEditForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Edit Avatar";
            this.panelPictureBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void TrackBarZoom_Scroll(object sender, EventArgs e)
        {
            zoomFactor = (trackBarZoom.Value - 1) * 0.049f + 0.1f;
            if (zoomFactor <= 0) zoomFactor = 0.1f;
            UpdateImage();
        }

        private void BtnRotate_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }

            rotationAngle = (rotationAngle + 90f) % 360f;
            UpdateImage();
        }

        private void PictureBoxEdit_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point scrollOffset = panelPictureBox.AutoScrollPosition;
                int imageX = e.X - scrollOffset.X;
                int imageY = e.Y - scrollOffset.Y;
                if (cropArea.Contains(imageX, imageY))
                {
                    isDraggingCrop = true;
                    lastMousePosition = new Point(imageX, imageY);
                }
            }
        }

        private void PictureBoxEdit_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingCrop)
            {
                Point scrollOffset = panelPictureBox.AutoScrollPosition;
                int imageX = e.X - scrollOffset.X;
                int imageY = e.Y - scrollOffset.Y;
                int dx = imageX - lastMousePosition.X;
                int dy = imageY - lastMousePosition.Y;

                // Cập nhật vị trí mới của cropArea dựa trên di chuyển chuột
                cropArea.X += dx;
                cropArea.Y += dy;

                // Giới hạn cropArea trong bounds của displayedImage
                cropArea.X = Math.Max(0, Math.Min(cropArea.X, displayedImage.Width - cropArea.Width));
                cropArea.Y = Math.Max(0, Math.Min(cropArea.Y, displayedImage.Height - cropArea.Height));

                // Cập nhật lastMousePosition với tọa độ mới trên displayedImage
                lastMousePosition = new Point(imageX, imageY);

                pictureBoxEdit.Invalidate();
            }
        }

        private void PictureBoxEdit_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraggingCrop = false;
            }
        }

        private void PictureBoxEdit_Paint(object sender, PaintEventArgs e)
        {
            if (displayedImage != null)
            {
                // Lấy vị trí cuộn hiện tại của panelPictureBox
                Point scrollOffset = panelPictureBox.AutoScrollPosition;

                // Điều chỉnh tọa độ của cropArea dựa trên vị trí cuộn
                int adjustedX = cropArea.X + scrollOffset.X;
                int adjustedY = cropArea.Y + scrollOffset.Y;

                // Vẽ vùng chọn với tọa độ đã điều chỉnh
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(pen, adjustedX, adjustedY, cropArea.Width, cropArea.Height);
                }
            }
        }

        private void UpdateImage()
        {
            if (originalImage == null || originalImage.Width <= 0 || originalImage.Height <= 0) return;

            if (displayedImage != null && displayedImage != originalImage)
            {
                displayedImage.Dispose();
            }

            int newWidth = (int)(originalImage.Width * zoomFactor);
            int newHeight = (int)(originalImage.Height * zoomFactor);
            if (newWidth <= 0) newWidth = 1;
            if (newHeight <= 0) newHeight = 1;

            try
            {
                Bitmap tempImage = new Bitmap(newWidth, newHeight);
                using (Graphics g = Graphics.FromImage(tempImage))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }

                if (rotationAngle != 0)
                {
                    double radians = rotationAngle * Math.PI / 180;
                    int rotatedWidth = (int)(Math.Abs(Math.Cos(radians) * newWidth) + Math.Abs(Math.Sin(radians) * newHeight));
                    int rotatedHeight = (int)(Math.Abs(Math.Sin(radians) * newWidth) + Math.Abs(Math.Cos(radians) * newHeight));
                    Bitmap rotatedImage = new Bitmap(rotatedWidth, rotatedHeight);

                    using (Graphics g = Graphics.FromImage(rotatedImage))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.TranslateTransform(rotatedWidth / 2f, rotatedHeight / 2f);
                        g.RotateTransform(rotationAngle);
                        g.TranslateTransform(-rotatedWidth / 2f, -rotatedHeight / 2f);
                        g.DrawImage(tempImage, (rotatedWidth - newWidth) / 2, (rotatedHeight - newHeight) / 2, newWidth, newHeight);
                    }
                    tempImage.Dispose();
                    displayedImage = rotatedImage;
                }
                else
                {
                    displayedImage = tempImage;
                }

                pictureBoxEdit.Image = displayedImage;
                pictureBoxEdit.Size = displayedImage.Size;
                pictureBoxEdit.Invalidate();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Error updating image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }

            if (originalImage == null) return;

            // Tạo chuỗi log cho vùng chọn trên displayedImage
            string logMessage = $"Selected Crop Area (on displayedImage):\n";
            logMessage += $"X={cropArea.X}, Y={cropArea.Y}, Width={cropArea.Width}, Height={cropArea.Height}\n\n";

            // Tính offset của ảnh phóng to trong canvas xoay
            float zoomedWidth = originalImage.Width * zoomFactor;
            float zoomedHeight = originalImage.Height * zoomFactor;
            double radians = rotationAngle * Math.PI / 180;
            int rotatedWidth = (int)(Math.Abs(Math.Cos(radians) * zoomedWidth) + Math.Abs(Math.Sin(radians) * zoomedHeight));
            int rotatedHeight = (int)(Math.Abs(Math.Sin(radians) * zoomedWidth) + Math.Abs(Math.Cos(radians) * zoomedHeight));
            float offsetX = (rotatedWidth - zoomedWidth) / 2f;
            float offsetY = (rotatedHeight - zoomedHeight) / 2f;

            // Biến đổi hai điểm: topLeft và bottomRight của cropArea về tọa độ ảnh gốc
            PointF topLeft = new PointF(cropArea.X, cropArea.Y);
            PointF bottomRight = new PointF(cropArea.X + cropArea.Width, cropArea.Y + cropArea.Height);
            using (Matrix matrix = new Matrix())
            {
                matrix.Translate(-offsetX, -offsetY); // Loại bỏ offset
                matrix.Rotate(-rotationAngle); // Xoay ngược
                matrix.Translate(-zoomedWidth / 2f, -zoomedHeight / 2f); // Đưa về tâm ảnh phóng to
                matrix.Scale(1 / zoomFactor, 1 / zoomFactor); // Thu nhỏ về kích thước gốc
                matrix.Translate(originalImage.Width / 2f, originalImage.Height / 2f); // Đưa về tâm ảnh gốc
                matrix.TransformPoints(new[] { topLeft, bottomRight });
            }

            // Tính mappedX, mappedY, mappedWidth, mappedHeight
            float mappedX = Math.Min(topLeft.X, bottomRight.X);
            float mappedY = Math.Min(topLeft.Y, bottomRight.Y);
            float mappedWidth = Math.Abs(bottomRight.X - topLeft.X);
            float mappedHeight = Math.Abs(bottomRight.Y - topLeft.Y);

            // Giới hạn trong kích thước ảnh gốc
            mappedX = Math.Max(0, Math.Min(mappedX, originalImage.Width - mappedWidth));
            mappedY = Math.Max(0, Math.Min(mappedY, originalImage.Height - mappedHeight));
            mappedWidth = Math.Min(mappedWidth, originalImage.Width - mappedX);
            mappedHeight = Math.Min(mappedHeight, originalImage.Height - mappedY);

            Rectangle sourceCropArea = new Rectangle((int)mappedX, (int)mappedY, (int)mappedWidth, (int)mappedHeight);

            // Thêm log cho vùng cắt trên originalImage
            logMessage += $"Cropped Area (on originalImage):\n";
            logMessage += $"X={sourceCropArea.X}, Y={sourceCropArea.Y}, Width={sourceCropArea.Width}, Height={sourceCropArea.Height}\n";
            logMessage += $"Original Image Size: Width={originalImage.Width}, Height={originalImage.Height}\n";
            logMessage += $"Zoom Factor: {zoomFactor}, Rotation Angle: {rotationAngle}";

            // Hiển thị log trong MessageBox
            //MessageBox.Show(logMessage, "Crop Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Cắt từ ảnh gốc với kích thước chính xác
            Bitmap cropped = new Bitmap(sourceCropArea.Width, sourceCropArea.Height);
            using (Graphics g = Graphics.FromImage(cropped))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, new Rectangle(0, 0, sourceCropArea.Width, sourceCropArea.Height), sourceCropArea, GraphicsUnit.Pixel);
            }

            // Xoay ảnh đã cắt chỉ khi cần thiết
            CroppedImage = (rotationAngle == 0) ? (Image)cropped.Clone() : RotateImage(cropped, rotationAngle);
            cropped.Dispose();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private Image CropImage(Image image, Rectangle cropArea)
        {
            if (image == null || cropArea.Width <= 0 || cropArea.Height <= 0) return null;

            // Ensure crop area is within image bounds
            cropArea.X = Math.Max(0, Math.Min(cropArea.X, image.Width - 1));
            cropArea.Y = Math.Max(0, Math.Min(cropArea.Y, image.Height - 1));
            cropArea.Width = Math.Min(cropArea.Width, image.Width - cropArea.X);
            cropArea.Height = Math.Min(cropArea.Height, image.Height - cropArea.Y);

            if (cropArea.Width <= 0 || cropArea.Height <= 0) return null;

            Bitmap bmpImage = new Bitmap(image);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private Bitmap RotateImage(Bitmap image, float angle)
        {
            if (angle == 0) return (Bitmap)image.Clone();

            double radians = angle * Math.PI / 180;
            int rotatedWidth = (int)(Math.Abs(Math.Cos(radians) * image.Width) + Math.Abs(Math.Sin(radians) * image.Height));
            int rotatedHeight = (int)(Math.Abs(Math.Sin(radians) * image.Width) + Math.Abs(Math.Cos(radians) * image.Height));
            Bitmap rotatedImage = new Bitmap(rotatedWidth, rotatedHeight);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.TranslateTransform(rotatedWidth / 2f, rotatedHeight / 2f);
                g.RotateTransform(angle);
                g.TranslateTransform(-image.Width / 2f, -image.Height / 2f);
                g.DrawImage(image, 0, 0);
            }
            return rotatedImage;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                originalImage?.Dispose();
                displayedImage?.Dispose();
                CroppedImage?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}