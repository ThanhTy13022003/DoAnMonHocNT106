using System.Collections.Generic;

namespace DoAnMonHocNT106
{
    public static class Translator
    {
        private static Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "Tiếng Anh", new Dictionary<string, string>
                {
                    { "Bật/Tắt Nhạc Nền", "Toggle Background Music" },
                    { "Bật/Tắt Âm Thanh", "Toggle Sound" },
                    { "Thông tin người chơi", "Player Info" },
                    { "Giới thiệu", "Introduction" },
                    { "Bảng xếp hạng", "Leaderboard" },
                    { "Đóng", "Close" },
                    { "Lobby", "Lobby" },
                    { "PvE", "PvE" },
                    { "Setting", "Settings" },
                    { "Log Out", "Log Out" },
                    { "Sign In", "Sign In" },
                    { "Sign Up", "Sign Up" },
                    { "Forgot Password?", "Forgot Password?" },
                    { "Show password", "Show password" },
                    { "Thời gian", "Time" },
                    { "Tên người dùng", "Username" },
                    { "Trạng thái", "Status" },
                    { "Danh sách Users", "User List" },
                    { "Chat công khai", "Public Chat" },
                    { "Gửi", "Send" },
                    { "Confirm", "Confirm" },
                    { "Email", "Email" },
                    { "Password", "Password" },
                    { "Re-password", "Re-password" },
                    { "Login With Gmail", "Login With Gmail" },
                    { "Login With PhoneNumber", "Login With PhoneNumber" },
                    { "Login With Facebook", "Login With Facebook" },
                    { "Thắng", "Wins" },
                    { "Thua", "Losses" },
                    { "Hòa", "Draws" },
                    { "Hết thời gian", "Timeouts" },
                    { "Lần cuối online", "Last Online" },
                    { "Ngôn ngữ: ", "Language: " }
                }
            },
            {
                "Tiếng Việt", new Dictionary<string, string>
                {
                    { "Bật/Tắt Nhạc Nền", "Bật/Tắt Nhạc Nền" },
                    { "Bật/Tắt Âm Thanh", "Bật/Tắt Âm Thanh" },
                    { "Thông tin người chơi", "Thông tin người chơi" },
                    { "Giới thiệu", "Giới thiệu" },
                    { "Bảng xếp hạng", "Bảng xếp hạng" },
                    { "Đóng", "Đóng" },
                    { "Lobby", "Lobby" },
                    { "PvE", "PvE" },
                    { "Setting", "Cài Đặt" },
                    { "Log Out", "Đăng Xuất" },
                    { "Sign In", "Đăng Nhập" },
                    { "Sign Up", "Đăng Ký" },
                    { "Forgot Password?", "Quên Mật Khẩu?" },
                    { "Show password", "Hiển thị mật khẩu" },
                    { "Thời gian", "Thời gian" },
                    { "Tên người dùng", "Tên người dùng" },
                    { "Trạng thái", "Trạng thái" },
                    { "Danh sách Users", "Danh sách Users" },
                    { "Chat công khai", "Chat công khai" },
                    { "Gửi", "Gửi" },
                    { "Confirm", "Xác Nhận" },
                    { "Email", "Email" },
                    { "Password", "Mật Khẩu" },
                    { "Re-password", "Nhập Lại Mật Khẩu" },
                    { "Login With Gmail", "Đăng Nhập Bằng Gmail" },
                    { "Login With PhoneNumber", "Đăng Nhập Bằng Số Điện Thoại" },
                    { "Login With Facebook", "Đăng Nhập Bằng Facebook" },
                    { "Thắng", "Thắng" },
                    { "Thua", "Thua" },
                    { "Hòa", "Hòa" },
                    { "Hết thời gian", "Hết thời gian" },
                    { "Lần cuối online", "Lần cuối online" },
                    { "Ngôn ngữ: ", "Ngôn ngữ: " }
                }
            },
            {
                "Tiếng Trung Quốc", new Dictionary<string, string>
                {
                    { "Bật/Tắt Nhạc Nền", "切换背景音乐" },
                    { "Bật/Tắt Âm Thanh", "切换声音" },
                    { "Thông tin người chơi", "玩家信息" },
                    { "Giới thiệu", "介绍" },
                    { "Bảng xếp hạng", "排行榜" },
                    { "Đóng", "关闭" },
                    { "Lobby", "大厅" },
                    { "PvE", "人机对战" },
                    { "Setting", "设置" },
                    { "Log Out", "退出" },
                    { "Sign In", "登录" },
                    { "Sign Up", "注册" },
                    { "Forgot Password?", "忘记密码？" },
                    { "Show password", "显示密码" },
                    { "Thời gian", "时间" },
                    { "Tên người dùng", "用户名" },
                    { "Trạng thái", "状态" },
                    { "Danh sách Users", "用户列表" },
                    { "Chat công khai", "公共聊天" },
                    { "Gửi", "发送" },
                    { "Confirm", "确认" },
                    { "Email", "电子邮件" },
                    { "Password", "密码" },
                    { "Re-password", "再次输入密码" },
                    { "Login With Gmail", "使用Gmail登录" },
                    { "Login With PhoneNumber", "使用电话号码登录" },
                    { "Login With Facebook", "使用Facebook登录" },
                    { "Thắng", "胜" },
                    { "Thua", "负" },
                    { "Hòa", "平" },
                    { "Hết thời gian", "超时" },
                    { "Lần cuối online", "最后在线" },
                    { "Ngôn ngữ: ", "语言： " }
                }
            },
            {
                "Tiếng Nga", new Dictionary<string, string>
                {
                    { "Bật/Tắt Nhạc Nền", "Переключить фоновую музыку" },
                    { "Bật/Tắt Âm Thanh", "Переключить звук" },
                    { "Thông tin người chơi", "Информация об игроке" },
                    { "Giới thiệu", "Введение" },
                    { "Bảng xếp hạng", "Таблица лидеров" },
                    { "Đóng", "Закрыть" },
                    { "Lobby", "Лобби" },
                    { "PvE", "PvE" },
                    { "Setting", "Настройки" },
                    { "Log Out", "Выйти" },
                    { "Sign In", "Войти" },
                    { "Sign Up", "Зарегистрироваться" },
                    { "Forgot Password?", "Забыли пароль?" },
                    { "Show password", "Показать пароль" },
                    { "Thời gian", "Время" },
                    { "Tên người dùng", "Имя пользователя" },
                    { "Trạng thái", "Статус" },
                    { "Danh sách Users", "Список пользователей" },
                    { "Chat công khai", "Общий чат" },
                    { "Gửi", "Отправить" },
                    { "Confirm", "Подтвердить" },
                    { "Email", "Электронная почта" },
                    { "Password", "Пароль" },
                    { "Re-password", "Повторить пароль" },
                    { "Login With Gmail", "Войти через Gmail" },
                    { "Login With PhoneNumber", "Войти через номер телефона" },
                    { "Login With Facebook", "Войти через Facebook" },
                    { "Thắng", "Победы" },
                    { "Thua", "Поражения" },
                    { "Hòa", "Ничьи" },
                    { "Hết thời gian", "Тайм-ауты" },
                    { "Lần cuối online", "Последний раз онлайн" },
                    { "Ngôn ngữ: ", "Язык: " }
                }
            }
        };

        /// <summary>
        /// Hàm dịch văn bản dựa trên ngôn ngữ hiện tại trong Settings.
        /// </summary>
        /// <param name="text">Văn bản cần dịch</param>
        /// <returns>Bản dịch nếu có, ngược lại trả về văn bản gốc</returns>
        public static string Translate(string text)
        {
            string language = Properties.Settings.Default["Language"]?.ToString() ?? "Tiếng Anh";
            if (translations.ContainsKey(language) && translations[language].ContainsKey(text))
            {
                return translations[language][text];
            }
            return text;
        }
    }
}
