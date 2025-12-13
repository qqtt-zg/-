using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp3.Utils;

namespace WindowsFormsApp3.Utils
{
    /// <summary>
    /// 窗口位置管理工具类
    /// 负责窗口位置和状态的保存与恢复
    /// </summary>
    public static class WindowPositionManager
    {
        /// <summary>
        /// 保存窗口位置和状态
        /// </summary>
        /// <param name="form">要保存的窗体</param>
        /// <param name="previewExpanded">PDF预览是否展开</param>
        public static void SaveWindowPosition(Form form, bool previewExpanded)
        {
            try
            {
                // 🔧 详细记录窗口状态，便于调试
                LogHelper.Debug($"[WindowPositionManager] 当前窗口状态: WindowState={form.WindowState}, Location={form.Location}, Size={form.Size}, ClientSize={form.ClientSize}");

                if (form.WindowState == FormWindowState.Normal)
                {
                    AppSettings.MaterialFormX = form.Location.X;
                    AppSettings.MaterialFormY = form.Location.Y;
                    AppSettings.MaterialFormWidth = form.Size.Width;
                    AppSettings.MaterialFormHeight = form.Size.Height;

                    LogHelper.Debug($"[WindowPositionManager] 保存窗口正常状态: Location={form.Location}, Size={form.Size}");
                }
                else if (form.WindowState == FormWindowState.Maximized)
                {
                    LogHelper.Debug("[WindowPositionManager] 窗口处于最大化状态，不保存位置和大小");
                }
                else
                {
                    // 🔧 修复：对于其他状态（如Minimized），仍然尝试保存位置和大小
                    LogHelper.Debug($"[WindowPositionManager] 窗口处于非Normal状态({form.WindowState})，但仍尝试保存位置信息");
                    AppSettings.MaterialFormX = form.Location.X;
                    AppSettings.MaterialFormY = form.Location.Y;
                    AppSettings.MaterialFormWidth = form.Size.Width;
                    AppSettings.MaterialFormHeight = form.Size.Height;
                    LogHelper.Debug($"[WindowPositionManager] 保存非Normal状态位置: Location={form.Location}, Size={form.Size}");
                }

                AppSettings.MaterialFormMaximized = form.WindowState == FormWindowState.Maximized;
                AppSettings.MaterialFormPreviewExpanded = previewExpanded;

                // 🔧 立即提交设置更改，确保窗口位置被持久化到文件
                AppSettings.CommitChanges();

                LogHelper.Debug($"[WindowPositionManager] 保存窗口状态: Maximized={AppSettings.MaterialFormMaximized}, PreviewExpanded={previewExpanded}");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[WindowPositionManager] 保存窗口位置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 恢复窗口位置和状态
        /// </summary>
        /// <param name="form">要恢复的窗体</param>
        public static void RestoreWindowPosition(Form form)
        {
            LogHelper.Debug("[WindowPositionManager] ========== 开始恢复窗口位置 ==========");

            try
            {
                // 🔧 移除重新加载逻辑，避免覆盖刚刚保存的窗口位置设置
                // AppSettings.Instance.ReloadAllSettings();
                LogHelper.Debug("[WindowPositionManager] 跳过重新加载，使用当前内存中的设置");

                // 记录窗体初始状态
                LogHelper.Debug($"[WindowPositionManager] 窗体初始状态: StartPosition={form.StartPosition}, Location={form.Location}, Size={form.Size}, WindowState={form.WindowState}");

                // 测试直接读取设置文件
                try
                {
                    int testX = AppSettings.MaterialFormX;
                    int testY = AppSettings.MaterialFormY;
                    LogHelper.Debug($"[WindowPositionManager] 测试读取AppSettings: X={testX}, Y={testY}");
                }
                catch (Exception settingsEx)
                {
                    LogHelper.Error($"[WindowPositionManager] 读取AppSettings失败: {settingsEx.Message}", settingsEx);
                }

                // 先读取所有设置值并记录日志，便于调试
                int savedX = AppSettings.MaterialFormX;
                int savedY = AppSettings.MaterialFormY;
                int savedWidth = AppSettings.MaterialFormWidth;
                int savedHeight = AppSettings.MaterialFormHeight;
                bool savedMaximized = AppSettings.MaterialFormMaximized;

                LogHelper.Debug($"[WindowPositionManager] 读取到的设置: X={savedX}, Y={savedY}, Width={savedWidth}, Height={savedHeight}, Maximized={savedMaximized}");

                // 恢复窗口最大化状态
                if (savedMaximized)
                {
                    form.WindowState = FormWindowState.Maximized;
                    LogHelper.Debug("[WindowPositionManager] 恢复窗口最大化状态");
                }
                else if (savedX >= 0 && savedY >= 0)
                {
                    LogHelper.Debug($"[WindowPositionManager] 满足位置恢复条件: X={savedX} >= 0, Y={savedY} >= 0");

                    // 强制设置为Manual模式，确保Location设置生效
                    form.StartPosition = FormStartPosition.Manual;
                    LogHelper.Debug("[WindowPositionManager] 设置StartPosition为Manual");

                    // 检查位置是否在屏幕范围内
                    var workingArea = Screen.PrimaryScreen.WorkingArea;
                    int x = Math.Max(workingArea.Left, Math.Min(savedX, workingArea.Right - form.MinimumSize.Width));
                    int y = Math.Max(workingArea.Top, Math.Min(savedY, workingArea.Bottom - form.MinimumSize.Height));

                    LogHelper.Debug($"[WindowPositionManager] 计算后的位置: ({x}, {y}), 工作区域: {workingArea}, 最小尺寸: {form.MinimumSize}");

                    // 先设置Location，再设置WindowState
                    form.Location = new Point(x, y);
                    LogHelper.Debug($"[WindowPositionManager] 已设置Location为: {form.Location}");

                    form.WindowState = FormWindowState.Normal;
                    LogHelper.Debug($"[WindowPositionManager] 已设置WindowState为: {form.WindowState}");

                    // 恢复大小（如果有效）
                    if (savedWidth > 0 && savedHeight > 0)
                    {
                        int width = Math.Max(form.MinimumSize.Width, savedWidth);
                        int height = Math.Max(form.MinimumSize.Height, savedHeight);

                        // 确保窗口大小不超过工作区域
                        width = Math.Min(width, workingArea.Width);
                        height = Math.Min(height, workingArea.Height);

                        form.Size = new Size(width, height);

                        LogHelper.Debug($"[WindowPositionManager] 恢复窗口正常状态: Location=({x}, {y}), Size=({width}, {height})");
                    }
                    else
                    {
                        LogHelper.Debug($"[WindowPositionManager] 大小值无效: Width={savedWidth}, Height={savedHeight}，只恢复位置");
                    }

                    // 验证最终状态
                    LogHelper.Debug($"[WindowPositionManager] 最终窗体状态: StartPosition={form.StartPosition}, Location={form.Location}, Size={form.Size}, WindowState={form.WindowState}");
                }
                else
                {
                    // 首次运行，居中显示
                    form.StartPosition = FormStartPosition.CenterScreen;
                    LogHelper.Debug($"[WindowPositionManager] 首次运行或位置无效，设置居中显示: X={savedX}, Y={savedY}");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[WindowPositionManager] 恢复窗口位置失败: {ex.Message}", ex);
                LogHelper.Error($"[WindowPositionManager] 异常堆栈: {ex.StackTrace}");
                // 发生异常时，确保窗口至少是可见的
                try
                {
                    form.StartPosition = FormStartPosition.CenterScreen;
                    LogHelper.Debug("[WindowPositionManager] 异常处理：设置StartPosition为CenterScreen");
                }
                catch (Exception fallbackEx)
                {
                    LogHelper.Error($"[WindowPositionManager] 异常处理也失败了: {fallbackEx.Message}", fallbackEx);
                }
            }

            LogHelper.Debug("[WindowPositionManager] ========== 窗口位置恢复完成 ==========");
        }

        /// <summary>
        /// 检查是否应该展开PDF预览
        /// </summary>
        /// <returns>如果上次保存时是展开状态则返回true</returns>
        public static bool ShouldExpandPreview()
        {
            try
            {
                bool shouldExpand = AppSettings.MaterialFormPreviewExpanded;
                LogHelper.Debug($"[WindowPositionManager] PDF预览状态检查: ShouldExpand={shouldExpand}");
                return shouldExpand;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[WindowPositionManager] 检查PDF预览状态失败: {ex.Message}", ex);
                return false; // 发生异常时默认不展开
            }
        }

        /// <summary>
        /// 重置窗口位置设置
        /// </summary>
        public static void ResetWindowPosition()
        {
            try
            {
                AppSettings.MaterialFormX = -1;
                AppSettings.MaterialFormY = -1;
                AppSettings.MaterialFormWidth = -1;
                AppSettings.MaterialFormHeight = -1;
                AppSettings.MaterialFormMaximized = false;
                AppSettings.MaterialFormPreviewExpanded = false;

                LogHelper.Debug("[WindowPositionManager] 窗口位置设置已重置");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[WindowPositionManager] 重置窗口位置失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查指定位置是否在屏幕可见范围内
        /// </summary>
        /// <param name="location">要检查的位置</param>
        /// <param name="size">窗口大小</param>
        /// <returns>如果位置可见则返回true</returns>
        public static bool IsPositionVisible(Point location, Size size)
        {
            try
            {
                Rectangle windowRect = new Rectangle(location, size);
                Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;

                return workingArea.IntersectsWith(windowRect);
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[WindowPositionManager] 检查位置可见性失败: {ex.Message}", ex);
                return true; // 发生异常时默认认为可见
            }
        }

        /// <summary>
        /// 获取安全的窗口位置（确保在屏幕可见范围内）
        /// </summary>
        /// <param name="desiredLocation">期望的位置</param>
        /// <param name="size">窗口大小</param>
        /// <returns>调整后的安全位置</returns>
        public static Point GetSafeLocation(Point desiredLocation, Size size)
        {
            try
            {
                var workingArea = Screen.PrimaryScreen.WorkingArea;

                int x = desiredLocation.X;
                int y = desiredLocation.Y;

                // 确保窗口不完全超出屏幕左边界
                if (x < workingArea.Left)
                    x = workingArea.Left;

                // 确保窗口不完全超出屏幕右边界
                if (x + size.Width > workingArea.Right)
                    x = workingArea.Right - size.Width;

                // 确保窗口不完全超出屏幕上边界
                if (y < workingArea.Top)
                    y = workingArea.Top;

                // 确保窗口不完全超出屏幕下边界
                if (y + size.Height > workingArea.Bottom)
                    y = workingArea.Bottom - size.Height;

                return new Point(x, y);
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[WindowPositionManager] 获取安全位置失败: {ex.Message}", ex);
                return desiredLocation; // 发生异常时返回原始位置
            }
        }

        /// <summary>
        /// 动画移动窗口位置
        /// </summary>
        /// <param name="form">要移动的窗体</param>
        /// <param name="targetX">目标X坐标</param>
        /// <param name="targetY">目标Y坐标</param>
        /// <param name="duration">动画持续时间（毫秒）</param>
        private static void AnimateWindowPosition(Form form, int targetX, int targetY, int duration)
        {
            if (duration <= 0)
            {
                // 如果时间为0或负数，直接设置位置
                form.Location = new Point(targetX, targetY);
                LogHelper.Debug($"[WindowPositionManager] 直接设置位置: ({targetX}, {targetY})");
                return;
            }

            var startTime = DateTime.Now;
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 16; // 约60FPS

            Point startPoint = form.Location;

            timer.Tick += (sender, EventArgs) =>
            {
                try
                {
                    var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                    if (elapsed >= duration)
                    {
                        timer.Stop();
                        timer.Dispose();
                        form.Location = new Point(targetX, targetY);
                        LogHelper.Debug($"[WindowPositionManager] 动画完成: 耗时{elapsed:F1}ms → 最终位置({targetX}, {targetY})");
                        return;
                    }

                    // 计算当前应该的位置（使用平滑插值）
                    float progress = (float)elapsed / duration;

                    // 使用缓动函数使动画更自然
                    float easedProgress = EaseOutCubic(progress);

                    int currentX = startPoint.X + (int)((targetX - startPoint.X) * easedProgress);
                    int currentY = startPoint.Y + (int)((targetY - startPoint.Y) * easedProgress);

                    form.Location = new Point(currentX, currentY);

                    LogHelper.Debug($"[WindowPositionManager] 动画进度: {progress:F1}% (eased: {easedProgress:F1}) → Location=({currentX}, {currentY})");
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"[WindowPositionManager] 动画失败: {ex.Message}");
                    timer.Stop();
                    timer.Dispose();
                }
            };
            timer.Start();
        }

        /// <summary>
        /// 缓出三次方函数，提供自然的减速效果
        /// </summary>
        /// <param name="progress">进度值(0-1)</param>
        /// <returns>缓动后的进度值</returns>
        private static float EaseOutCubic(float progress)
        {
            return 1 - (float)Math.Pow(1 - progress, 3);
        }

        /// <summary>
        /// 根据跳跃距离计算动画时间
        /// </summary>
        /// <param name="jumpDistance">跳跃距离（像素）</param>
        /// <returns>动画持续时间（毫秒）</returns>
        private static int CalculateJumpTime(double jumpDistance)
        {
            // 🔧 进一步优化：几乎瞬间完成，但保留最小动画效果
            // 基础动画时间：0ms（完全瞬间）
            int baseTime = 0;

            // 大幅缩短时间范围：0-5ms
            int maxTime = 5;

            // 更激进的时间计算：1000px = 1ms, 5000px = 5ms
            int jumpTime = Math.Min(maxTime, Math.Max(baseTime, (int)(jumpDistance / 1000)));

            LogHelper.Debug($"[WindowPositionManager] 距离{jumpDistance:F1}px，计算动画时间: {jumpTime}ms（进一步优化版）");

            return jumpTime;
        }

        /// <summary>
        /// 恢复窗口位置和状态（平滑动画版本）
        /// </summary>
        /// <param name="form">要恢复的窗体</param>
        public static void RestoreWindowPositionSmooth(Form form)
        {
            try
            {
                LogHelper.Debug("[WindowPositionManager] ========== 开始平滑恢复窗口位置 ==========");

                // 先读取保存的设置
                int savedX = AppSettings.MaterialFormX;
                int savedY = AppSettings.MaterialFormY;
                int savedWidth = AppSettings.MaterialFormWidth;
                int savedHeight = AppSettings.MaterialFormHeight;
                bool savedMaximized = AppSettings.MaterialFormMaximized;

                LogHelper.Debug($"[WindowPositionManager] 读取到的设置: X={savedX}, Y={savedY}, Width={savedWidth}, Height={savedHeight}, Maximized={savedMaximized}");

                if (savedMaximized)
                {
                    form.WindowState = FormWindowState.Maximized;
                    LogHelper.Debug("[WindowPositionManager] 恢复窗口最大化状态");
                    LogHelper.Debug("[WindowPositionManager] ========== 窗口位置恢复完成 ==========");
                    return;
                }

                // 🔧 如果无保存的位置，直接返回
                if (savedX < 0 || savedY < 0)
                {
                    LogHelper.Debug("[WindowPositionManager] 无保存位置，保持默认居中显示");
                    LogHelper.Debug("[WindowPositionManager] ========== 窗口位置恢复完成 ==========");
                    return;
                }

                // 计算目标位置
                var workingArea = Screen.PrimaryScreen.WorkingArea;
                int targetX = Math.Max(workingArea.Left, Math.Min(savedX, workingArea.Right - form.MinimumSize.Width));
                int targetY = Math.Max(workingArea.Top, Math.Min(savedY, workingArea.Bottom - form.MinimumSize.Height));

                // 恢复大小（如果有效）
                if (savedWidth > 0 && savedHeight > 0)
                {
                    int width = Math.Max(form.MinimumSize.Width, savedWidth);
                    int height = Math.Max(form.MinimumSize.Height, savedHeight);

                    // 确保窗口大小不超过工作区域
                    width = Math.Min(width, workingArea.Width);
                    height = Math.Min(height, workingArea.Height);

                    form.Size = new Size(width, height);
                }

                // 计算当前起始位置（屏幕中心）到目标位置的距离
                Point centerPosition = new Point(workingArea.Width / 2 - form.Size.Width / 2,
                                               workingArea.Height / 2 - form.Size.Height / 2);

                // 计算跳跃距离
                int jumpDistanceX = Math.Abs(targetX - centerPosition.X);
                int jumpDistanceY = Math.Abs(targetY - centerPosition.Y);
                double jumpDistance = Math.Sqrt(jumpDistanceX * jumpDistanceX + jumpDistanceY * jumpDistanceY);

                LogHelper.Debug($"[WindowPositionManager] 中心位置: ({centerPosition.X}, {centerPosition.Y}), 目标位置: ({targetX}, {targetY})");
                LogHelper.Debug($"[WindowPositionManager] 跳跃距离: {jumpDistance:F1}px");

                // 🔧 计算跳跃时间（根据跳跃距离动态调整）
                int jumpTime = CalculateJumpTime(jumpDistance);
                LogHelper.Debug($"[WindowPositionManager] 计算跳跃时间: {jumpTime}ms");

                // 执行平滑位置移动动画
                AnimateWindowPosition(form, targetX, targetY, jumpTime);
            }
            catch (Exception ex)
            {
                LogHelper.Error($"[WindowPositionManager] 平滑恢复窗口位置失败: {ex.Message}", ex);

                // 异常处理：直接跳到目标位置
                try
                {
                    var workingArea = Screen.PrimaryScreen.WorkingArea;
                    int savedX = AppSettings.MaterialFormX;
                    int savedY = AppSettings.MaterialFormY;

                    int x = Math.Max(workingArea.Left, Math.Min(savedX, workingArea.Right - form.MinimumSize.Width));
                    int y = Math.Max(workingArea.Top, Math.Min(savedY, workingArea.Bottom - form.MinimumSize.Height));

                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(x, y);
                    form.WindowState = FormWindowState.Normal;

                    LogHelper.Debug($"[WindowPositionManager] 异常处理：直接跳到目标位置: ({x}, {y})");
                }
                catch (Exception fallbackEx)
                {
                    LogHelper.Error($"[WindowPositionManager] 异常处理也失败了: {fallbackEx.Message}", fallbackEx);
                }
            }
        }
    }
}