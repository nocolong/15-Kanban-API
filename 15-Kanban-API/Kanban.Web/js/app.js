angular.module('kanban', ['ngResource']);
angular.module('kanban').value('apiUrl', 'http://localhost:52027/api');

angular.module('kanban').controller('IndexController', function ($scope, $resource, apiUrl) {

    var CardsResource = $resource(apiUrl + '/cards/cardId', { cardId: '@CardId' }, {
        'cards': {
            method: 'GET',
            url: apiUrl + '/lists/:listId/cards',
            isArray: true
        }
    });

    var ListsResource = $resource(apiUrl + '/lists/:listId', { listId: '@ListId' },
        {
            'update': {method: 'PUT'}
        });

    $scope.data = {
        newList: {},
        newCard: {}
    };

    $scope.addList = function () {
        ListsResource.save($scope.data.newList, function (data) {
            $scope.data.newList = {};
            activate();
        });
    };

    $scope.addCard = function (list) {
        list.newCard.ListId = list.ListId;
        CardsResource.save(list.newCard, function (data) {            
            activate();
        });
    };

    $scope.removeList = function (list) {
        list.$remove(function (data) {
            activate();
        });
    };
    $scope.saveList = function (list) {
        list.$update(function () {
            activate();
        });
    };
    $scope.removeCard = function (card) {

        card.$remove(function (data) {
            activate();
        });
    };

    activate();

    function activate() {
        var lists = ListsResource.query(function (data) {
            $scope.data.lists = data;

            $scope.data.lists.forEach(function (list) {
                list.cards = CardsResource.cards({ listId: list.ListId });
            });
        });

        $scope.lists = lists;
    }

    $scope.newList = {};
    $scope.addList = function () {
        ListResource.save($scope.newList, function () {
            alert('save successful');
            activate();
        });
    };
  
    activate();
});