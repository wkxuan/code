insert into menutree
select rownum,substr(id,3,1),id,name from MENU  



    --系统管理
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101001, '系统参数定义', 'XTGL/CONFIG/Config', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101002, '收款方式定义', 'XTGL/PAY/Pay', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101003, '组织结构定义', 'XTGL/ORG/Org', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101004, '分店定义', 'XTGL/BRANCH/Branch', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101005, '费用项目定义', 'XTGL/FEESUBJECT/FeeSubject', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101006, '定义角色', 'XTGL/SYSUSER/Sysuser', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101007, '定义角色组', 'XTGL/ROLE/RoleList', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101008, '滞纳金规则定义', 'XTGL/LATEFEERULE/LateFeeRule', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101009, '收费规则定义', 'XTGL/FEERULE/FeeRule', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101010, '财务月区间定义', 'XTGL/PERIOD/Period', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101011, '合作方式定义', 'XTGL/OPERATIONRULE/Operationrule', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101012, '付款方式定义', 'XTGL/FKFS/Fkfs', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(101013, '业态定义', 'XTGL/CATEGORY/Category', 1, 1);
    --品牌商户
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(102001, '商户维护', 'SHGL/MERCHANT/MerchantList', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(102002, '品牌维护', 'PPGL/BRAND/BrandList', 1, 1);
    --物业管理
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(103001, '能源设备档案定义', 'XTGL/ENERGYFILES/EnergyFiles', 1, 1);
    --资产管理
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(104001, '资产面积变更', 'DPGL/ASSETCHANGE/AssetChangeList', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(104002, '资产拆分', 'DPGL/ASSETSPILT/AssetSpiltList', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(104003, '楼层定义', 'XTGL/FLOOR/Floor', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(104004, '资产单元定义', 'XTGL/SHOP/Shop', 1, 1);
    --零售管理
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(105001, '收银终端定义', 'XTGL/STATION/Station', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(105002, '销售码维护', 'SPGL/GOODS/GoodsList', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(105003, '商品分类定义', 'XTGL/GOODS_KIND/Goods_Kind', 1, 1);
    --合同管理
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(106001, '联营合同维护', 'HTGL/LYHT/HtList', 1, 1);
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(106002, '租赁合同维护', 'HTGL/ZLHT/HtList', 1, 1);
    --结算管理
insert into MENU(ID, NAME, URL, STATUS, TYPE) values(107001, '保证金返还', 'JSGL/BILL_RETURN/Bill_ReturnList', 1, 1);