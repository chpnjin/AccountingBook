DROP TABLE IF EXISTS voucher;
DROP TABLE IF EXISTS voucher_detail;
-- 傳票單頭
CREATE TABLE voucher(
    no VARCHAR(20) NOT NULL UNIQUE COMMENT '傳票編號',
    entry_date DATE NOT NULL COMMENT '交易日期',
    voucher_type ENUM('opening', 'income', 'expense', 'transfer') NOT NULL COMMENT '傳票類型',
    summary TEXT COMMENT '傳票摘要',
    handler INT NOT NULL COMMENT '經辦人員ID(user.id)',
    reviewer INT NOT NULL COMMENT '複核人員ID(user.id)',
    status ENUM('unapproved', 'approving', 'approved', 'posted', 'void') NOT NULL COMMENT '狀態',
    create_user INT NOT NULL COMMENT '創建人員ID(user.id)',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '創建時間',
    last_update_user INT NOT NULL COMMENT '更新人員ID(user.id)',
    last_update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '最後修改時間',
    PRIMARY KEY (no)
);

-- 傳票單身
CREATE TABLE voucher_detail(
    no VARCHAR(20) NOT NULL COMMENT '傳票編號(voucher.no)',
    seq INT NOT NULL COMMENT '項目序號',
    accountNo INT NOT NULL COMMENT '科目代碼 (account.id)',
    summary TEXT COMMENT '項目摘要',
    debit_amount DECIMAL(18,2) DEFAULT 0.00 COMMENT '借方金額',
    credit_amount DECIMAL(18,2) DEFAULT 0.00 COMMENT '貸方金額',
    PRIMARY KEY (no, seq),
    FOREIGN KEY (no) REFERENCES voucher(no) ON DELETE CASCADE
);