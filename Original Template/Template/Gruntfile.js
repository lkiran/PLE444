module.exports = function (grunt) {
  grunt.initConfig({
    liquid: {
      options: {
        includes: ['templates/includes', 'templates/layouts'],
        products: [
        {
          name: "Wonderflonium",
          price: "$9.99",
          description: "Great for building freeze rays!"
        }
        ]
      },
      pages: {
        files: [
        {
          expand: true,
          flatten: true,
          src: 'templates/*.liquid',
          dest: '.',
          ext: '.html'
        }
        ]
      }
    },


    less: {
      development: {
        options: {
           // dumpLineNumbers: true,
           // sourceMap: true,
           // sourceMapRootpath: "",
           // outputSourceFiles: true,
          paths: ["assets/less"]
        },
        files: {
          "assets/css/styles.css": "assets/less/styles.less"
        }
      }
    },

    // sass: {
    //     options: {
    //         sourceMap: true
    //     },
    //     dist: {
    //         files: {
    //             'assets/css/styles.css': 'assets/sass/styles.scss'
    //         }
    //     }
    // },


    watch: {

      liquidTask: {
        files: ['templates/{,*/}*.liquid'],
        tasks: ['liquid'],
        options: {
          spawn: true,
        },
      },
      lessTask: {
        files: ['assets/less/{,*/}*.less'],
        tasks: ['less'],
        options: {
          spawn: true,
        },
      },
      // sassTask: {
      //   files: ['assets/sass/{,*/}*.scss'],
      //   tasks: ['sass'],
      //   options: {
      //     spawn: true,
      //   }
      // }
    }



  });


  grunt.registerTask('nightswatch', 'my watch has begun', function () {
      var tasks = ['watch'];
      grunt.option('force', true);
      grunt.task.run(tasks);
  });


  grunt.loadNpmTasks('grunt-contrib-less');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-liquid');
  // grunt.loadNpmTasks('grunt-sass');
  
  var Liquid = require('./node_modules/grunt-liquid/node_modules/liquid-node/lib/liquid');
  Liquid.Template.registerFilter({
    asset_url: function (input) {
      return 'assets/'+input;
    },
    bower_url: function (input) {
      return 'bower_components/'+input;
    },
    stylesheet_tag: function (input) {
      return '<link type="text/css" href="'+input+'" rel="stylesheet">';
    },
    script_tag: function (input) {
      return '<script type="text/javascript" src="'+input+'"></script>';
    },
    img_loc: function (input) {
      return input;
      return 'http://placehold.it/300&text=Placeholder';
    }
  });


  grunt.registerTask('default', ['sass']);
  grunt.registerTask('default', ['liquid', 'less:development']);

  grunt.registerTask('watchLiquidAndLe', ['liquid']);

};
