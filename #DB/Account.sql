DROP TABLE IF EXISTS account;
CREATE TABLE account (
    id INT PRIMARY KEY AUTO_INCREMENT COMMENT '主鍵，科目唯一識別碼',
    no VARCHAR(20) NOT NULL UNIQUE COMMENT '科目編號，通常是會計科目的標識號，如 "1010" 或 "2020"',
    name NVARCHAR(100) NOT NULL COMMENT '科目名稱，例如 "現金"、"應收帳款"',
    type ENUM('Asset', 'Liability', 'Equity', 'Revenue', 'Expense') NOT NULL COMMENT '科目類型，決定科目在財務報表中的位置',
    main_id INT COMMENT '主科目ID，用於表示科目層次結構', FOREIGN KEY (main_id) REFERENCES account(id),
    description TEXT COMMENT '描述，提供科目的詳細說明',
    active BOOLEAN DEFAULT TRUE COMMENT '是否是活動科目（是否在使用中）',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '創建時間',
    last_updated_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '最後修改時間'
);