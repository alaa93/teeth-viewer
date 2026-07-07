mergeInto(LibraryManager.library, {
  TeethViewer_DisableContextMenu: function () {
    if (typeof document !== 'undefined') {
      document.addEventListener('contextmenu', function (e) { e.preventDefault(); }, false);
    }
  }
});
