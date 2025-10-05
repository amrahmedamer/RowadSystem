window.bootstrapModal = {
    show: function (id) {
        var modalElement = document.querySelector(id);
        if (!modalElement) return; // لو العنصر مش موجود ماتعملش حاجة

        var myModal = bootstrap.Modal.getOrCreateInstance(modalElement); // تجنب تكرار الـ modal instance
        myModal.show();
    },
    hide: function (id) {
        var modalElement = document.querySelector(id);
        if (!modalElement) return; // نفس الفكرة لو العنصر مش موجود

        var myModal = bootstrap.Modal.getOrCreateInstance(modalElement);
        myModal.hide();

        // تنظيف الخلفية (backdrop) لو موجودة بعد إغلاق المودال
        setTimeout(() => {
            document.querySelectorAll('.modal-backdrop').forEach(b => b.remove());
            document.body.classList.remove('modal-open');
            document.body.style.removeProperty('padding-right');
        }, 50);
    }
};