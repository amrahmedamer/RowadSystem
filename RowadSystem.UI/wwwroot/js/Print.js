//window.printBarcode = function (base64Image) {
//    if (!base64Image) {
//        console.error("الصورة غير متاحة للطباعة.");
//        return;
//    }

//    // إنشاء iframe مخفي
//    var iframe = document.createElement('iframe');
//    iframe.style.position = 'absolute';
//    iframe.style.width = '0px';
//    iframe.style.height = '0px';
//    iframe.style.border = '0';
//    document.body.appendChild(iframe);

//    // كتابة محتوى الصورة داخل iframe
//    var doc = iframe.contentWindow.document;
//    doc.open();
//    doc.write('<html><head><title>Barcode</title></head><body style="margin:0;padding:0;">');
//    doc.write('<img src="data:image/png;base64,' + base64Image + '" style="width:100%;height:auto;"/>');
//    doc.write('</body></html>');
//    doc.close();

//    // بعد التحميل اطبع ثم احذف iframe
//    iframe.onload = function () {
//        iframe.contentWindow.focus();
//        iframe.contentWindow.print();
//        document.body.removeChild(iframe);
//    };
//};

//window.printBarcode = function (base64Image, productName = "لمبة 200 وات ", productPrice = 30) {
//    if (!base64Image) {
//        console.error("الصورة غير متاحة للطباعة.");
//        return;
//    }

//    // إنشاء iframe مخفي
//    var iframe = document.createElement('iframe');
//    iframe.style.position = 'absolute';
//    iframe.style.width = '0px';
//    iframe.style.height = '0px';
//    iframe.style.border = '0';
//    document.body.appendChild(iframe);

//    // كتابة محتوى الصورة داخل iframe
//    var doc = iframe.contentWindow.document;
//    doc.open();
//    doc.write('<html><head><style>');
//    doc.write('body { margin: 0; padding: 0; font-family: Arial, sans-serif; text-align: center; }');
//    doc.write('.container { width: 100%; padding: 20px; box-sizing: border-box; }');
//    doc.write('.product-name { font-size: 22px; font-weight: bold; margin-bottom: 10px; }');
//    doc.write('.barcode { width: 250px; height: 250px; margin: 20px 0; }');
//    doc.write('.product-price { font-size: 20px; font-weight: bold; color: #333; margin-top: 10px; }');
//    doc.write('@media print { body { margin: 0; padding: 0; } }');  // منع أي تذييل أو إضافة أثناء الطباعة
//    doc.write('</style></head><body>');

//    // إضافة الاسم فوق الباركود
//    doc.write('<div class="container">');
//    doc.write('<div class="product-name">' + productName + '</div>');

//    // إضافة الباركود في المنتصف
//    doc.write('<img src="data:image/png;base64,' + base64Image + '" class="barcode" />');

//    // إضافة السعر تحت الباركود
//    doc.write('<div class="product-price">' + productPrice + ' LE </div>');
//    doc.write('</div>');

//    doc.write('</body></html>');
//    doc.close();

//    // بعد التحميل اطبع ثم احذف iframe
//    iframe.onload = function () {
//        iframe.contentWindow.focus();
//        iframe.contentWindow.print();
//    };

//    // تأكد من حذف الـ iframe بعد الطباعة
//    iframe.contentWindow.onafterprint = function () {
//        document.body.removeChild(iframe);
//    };
//};

//function printBarcode(base64Image, productName = "لمبة 200 وات", productPrice = 30) {
//    if (!base64Image) {
//        console.error("الصورة غير متاحة للطباعة.");
//        return;
//    }

//    // إنشاء iframe مخفي
//    var iframe = document.createElement('iframe');
//    iframe.style.position = 'absolute';
//    iframe.style.width = '0px';
//    iframe.style.height = '0px';
//    iframe.style.border = '0';
//    document.body.appendChild(iframe);

//    // كتابة محتوى الصورة داخل iframe
//    var doc = iframe.contentWindow.document;
//    doc.open();
//    doc.write('<html><head><style>');
//    doc.write('body { margin: 0; padding: 0; font-family: Arial, sans-serif; text-align: center; }');
//    doc.write('.container { width: 100%; padding: 20px; box-sizing: border-box; }');
//    doc.write('.product-name { font-size: 22px; font-weight: bold; margin-bottom: 15px; }');
//    doc.write('.barcode { width: 3cm; height: 4cm; margin: 10px 0;  }');
//    doc.write('.product-price { font-size: 20px; font-weight: bold; color: #333; margin-top: 10px; }');

//    // خصائص الطباعة
//    doc.write('@media print {');
//    doc.write('body { margin: 0; padding: 0; font-family: Arial, sans-serif; }');
//    doc.write('.container { text-align: center; }');
//    doc.write('header, footer, nav, aside { display: none !important; }');  // إخفاء الـ header, footer
//    doc.write('::after { content: ""; display: none; }');  // إخفاء التذييلات الإضافية مثل التاريخ والـ URL
//    doc.write('@page { margin: 0; }');  // إزالة الهوامش الافتراضية للطباعة
//    doc.write('}');
//    doc.write('</style></head><body>');

//    // إضافة الاسم فوق الباركود
//    doc.write('<div class="container">');
//    doc.write('<div class="product-name">' + productName + '</div>');

//    // إضافة الباركود في المنتصف
//    doc.write('<img src="data:image/png;base64,' + base64Image + '" class="barcode" style="width:100mm;height:auto;" />');

//    // إضافة السعر تحت الباركود
//    doc.write('<div class="product-price">' + productPrice + ' LE </div>');
//    doc.write('</div>');

//    doc.write('</body></html>');
//    doc.close();

//    // بعد التحميل اطبع ثم احذف iframe
//    iframe.onload = function () {
//        iframe.contentWindow.focus();
//        iframe.contentWindow.print();
//    };

//    // تأكد من حذف الـ iframe بعد الطباعة
//    iframe.contentWindow.onafterprint = function () {
//        document.body.removeChild(iframe);
//    };
//}

function printBarcode(base64Image, productName , productPrice ) {
    if (!base64Image) {
        console.error("الصورة غير متاحة للطباعة.");
        return;
    }

    // إنشاء iframe مخفي
    var iframe = document.createElement('iframe');
    iframe.style.position = 'absolute';
    iframe.style.width = '0px';
    iframe.style.height = '0px';
    iframe.style.border = '0';
    document.body.appendChild(iframe);

    // كتابة محتوى الصورة داخل iframe
    var doc = iframe.contentWindow.document;
    doc.open();
    doc.write('<html><head><style>');
    doc.write('body { margin: 0; padding: 0; font-family: Arial, sans-serif; text-align: center; }');
    doc.write('.container { width: 100%; padding: 5mm; box-sizing: border-box; }');
    doc.write('.product-name { font-size: 10px; font-weight: bold; margin-bottom: 2mm; }');  // حجم الخط للاسم
    doc.write('.barcode { width: 35mm; height: 20mm; margin: 2mm 0; }');  // حجم الباركود
    doc.write('.product-price { font-size: 9px; font-weight: bold; color: #333; margin-top: 2mm; }');  // حجم الخط للسعر

    // خصائص الطباعة
    doc.write('@media print {');
    doc.write('body { margin: 0; padding: 0; font-family: Arial, sans-serif; }');
    doc.write('.container { text-align: center; }');
    doc.write('header, footer, nav, aside { display: none !important; }');  // إخفاء الـ header, footer
    doc.write('::after { content: ""; display: none; }');  // إخفاء التذييلات الإضافية مثل التاريخ والـ URL
    doc.write('::before { content: ""; display: none; }'); // إخفاء أي محتوى إضافي قبل الطباعة
    doc.write('@page { margin: 0; size: 40mm 30mm; }');  // ضبط حجم الصفحة ليتناسب مع ملصقات 40mm x 30mm
    doc.write('body { page-break-before: always; }');  // منع تقسيم المحتوى بين صفحات متعددة
    doc.write('@page { size: auto; margin: 0; }');  // إزالة الهوامش والتأكد من أنه لا توجد مساحة إضافية
    doc.write('}');
    doc.write('</style></head><body>');

    // إضافة الاسم فوق الباركود
    doc.write('<div class="container">');
    doc.write('<div class="product-name">' + productName + '</div>');

    // إضافة الباركود في المنتصف
    doc.write('<img src="data:image/png;base64,' + base64Image + '" class="barcode" />');

    // إضافة السعر تحت الباركود
    doc.write('<div class="product-price">' + productPrice + ' LE </div>');
    doc.write('</div>');

    doc.write('</body></html>');
    doc.close();

    // بعد التحميل اطبع ثم احذف iframe
    iframe.onload = function () {
        iframe.contentWindow.focus();
        iframe.contentWindow.print();
    };

    // تأكد من حذف الـ iframe بعد الطباعة
    iframe.contentWindow.onafterprint = function () {
        document.body.removeChild(iframe);
    };
}
