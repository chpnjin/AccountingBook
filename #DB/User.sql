CREATE TABLE user (
    id INT PRIMARY KEY AUTO_INCREMENT COMMENT '主鍵，使用者唯一識別碼',
    name VARCHAR(50) NOT NULL UNIQUE COMMENT '使用者名稱',
    email VARCHAR(100) NOT NULL UNIQUE COMMENT '電子郵件，其中一個唯一識別',
    password VARCHAR(255) NOT NULL COMMENT '密碼，通常是加密後的',
    role_group VARCHAR(255) NOT NULL COMMENT '角色群組,可複選,使用"|"符號分隔',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '帳戶創建日期',
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '帳戶最後更新日期',
    status ENUM('Active', 'Inactive', 'Suspended') NOT NULL DEFAULT 'Active' COMMENT '狀態'
);

CREATE TABLE user_info (
    id INT PRIMARY KEY AUTO_INCREMENT COMMENT '主鍵，使用者唯一識別碼',
    full_name VARCHAR(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci COMMENT '使用者全名，可以是公司名或個人名',
    phone VARCHAR(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci COMMENT '聯絡電話'
) DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;