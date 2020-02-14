(() => {
    function directive() {

        const dir = {
            restrict: 'A',
            link: scope => {
                const watcher = scope.$watch(() => scope.page.menu.currentNode, (a, b) => {
                    if (a && b && a !== b && !scope.page.loading) {
                        setTimeout(() => {
                            const umbTreeIcon = document.querySelector('.umb-tree-item.current .umb-tree-icon');
                            umbTreeIcon.className = `${umbTreeIcon.className.split('sprTree')[0]}sprTree ${a.icon}`;
                        });
                    }
                }, true);

                scope.$on('destroy', () => watcher());
            }
        };

        return dir;
    }

    angular.module('stately.directives').directive('statelyHook', directive);
})();

