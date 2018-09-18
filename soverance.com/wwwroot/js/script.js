"use strict";

// JavaScript Document

( function( $ )
{
	var winObj = $( window ),
		bodyObj = $( 'body' ),
		headerObj = $( 'header' );

		winObj.on( 'load', function()
		{
			var $preloader = $( '.loader-wrapper' );
				
			$preloader.find( '.cssload-loader' ).fadeOut();
			$preloader.delay( 350 ).fadeOut( 'slow' );
		} );
		
 	/*----------------------------------------------------*/
	/* Adaptive Menu Width
	/*----------------------------------------------------*/

	var ulMenu1 = $( 'ul.menu' );
	winObj.on( 'resize', function()
	{		
		if( $( this ).width() > 992 )
		{
			ulMenu1.flexMenu();
		}
	} );

	if( winObj.width() > 992 )
	{
		ulMenu1.flexMenu();
	}

	headerObj.on( 'click', '.flexMenu-viewMore', function()
	{
		$( this ).toggleClass( 'active' );
	} );

	/*----------------------------------------------------*/
	/* WOW Initialize
	/*----------------------------------------------------*/
	new WOW().init();

	/*----------------------------------------------------*/
	/*	Animated Scroll To Top
	/*----------------------------------------------------*/
	var toTop = $( '#toTop' );
	toTop.on( 'click', function()
	{
		$( 'html, body' ).animate( {
			scrollTop: 0
		}, 600 );
		return false;
	} );

	winObj.on( 'scroll', function()
	{
		if( $( this ).scrollTop() != 0 )
		{
			toTop.fadeIn();
		}
		else
		{
			toTop.fadeOut();
		}
	} );

	/*----------------------------------------------------*/
	/* Toggle menu
	/*----------------------------------------------------*/
	headerObj.on( 'click', '.toggle_menu', function()
	{
		$( this ).toggleClass( 'open' );
		if( $( this ).hasClass( 'open' ) )
		{
			$( '.menu' ).addClass( 'open' );
			bodyObj.addClass('no-scroll');
		}
		else
		{
			$( '.menu' ).removeClass( 'open' );
			$( '.menu-item-has-children' ).removeClass( 'open-list' );
			bodyObj.removeClass( 'no-scroll' );
		}
	} );

	/*----------------------------------------------------*/
	/* Mobile Sub Menu
	/*----------------------------------------------------*/
	headerObj.on( 'click', '.menu.open a', function( e )
	{
		if( $( this ).siblings().length )
		{
			if( !$( this ).parent().hasClass( 'open-list' ) )
			{
				$( this ).parent().addClass( 'open-list' );
				e.preventDefault();
			}
			else
			{
				$( this ).parent().removeClass( 'open-list' );
			}
		}
	} );

	/*----------------------------------------------------*/
	/* Equal height
	/*----------------------------------------------------*/
	$( '.equal-height' ).matchHeight();

	/*----------------------------------------------------*/
	/* Sliders Settings
	/*----------------------------------------------------*/
	$( '.tabs-slider' ).slick( {
		slidesToShow: 4,
		slidesToScroll: 1,
		arrows: true,
		prevArrow: '<i class="fa fa-angle-left slick-arrow"></i>',
		nextArrow: '<i class="fa fa-angle-right slick-arrow"></i>',
		responsive: [{
			breakpoint: 1120,
			settings: {
				autoplay: true
			}
		},
			{
			breakpoint: 992,
			settings: {
				slidesToShow: 3
			}
		},
		{
			breakpoint: 768,
			settings: {
				slidesToShow: 2
			}
		},
		{
			breakpoint: 480,
			settings: {
				slidesToShow: 1
			}
		}
	]
	} );

	$('.tabs-slider').on( 'click', 'li', function() {
		$( this ).addClass( 'active' );
		if( $( this ).hasClass( 'active' ) ) {
			$('.tabs-slider li').removeClass( 'active' );
		}
	} );

	$( '.testimonial-slider' ).slick( {
		slidesToShow: 2,
		slidesToScroll: 1,
		autoplay: true,
		arrows: true,
		infinite: true,
		dots: true,
		prevArrow: '<i class="fa fa-angle-left slick-arrow"></i>',
		nextArrow: '<i class="fa fa-angle-right slick-arrow"></i>',
		adaptiveHeight: true,
		responsive: [{
			breakpoint: 992,
			settings: {
				slidesToShow: 1
			}
		}
	]
	} );

	$( '.game-img-slider' ).slick( {
		slidesToShow: 3,
		slidesToScroll: 1,
		autoplay: true,
		arrows: true,
		infinite: true,
		dots: false,
		prevArrow: '<i class="fa fa-angle-left slick-arrow"></i>',
		nextArrow: '<i class="fa fa-angle-right slick-arrow"></i>',
		adaptiveHeight: true,
		responsive: [{
			breakpoint: 992,
			settings: {
				slidesToShow: 2
			}
		},
		{
			breakpoint: 768,
			settings: {
				slidesToShow: 1
			}
		}
	]
	} );

	$( '.top-slider-bl' ).slick( {
		slidesToShow: 1,
		slidesToScroll: 1,
		autoplay: true,
		arrows: true,
		infinite: true,
		dots: false,
		prevArrow: '<i class="fa fa-angle-left slick-arrow"></i>',
		nextArrow: '<i class="fa fa-angle-right slick-arrow"></i>'
	} );

	$( '.post-slider' ).slick( {
		slidesToShow: 1,
		slidesToScroll: 1,
		autoplay: true,
		arrows: false,
		infinite: true,
		dots: true,
		prevArrow: '<i class="fa fa-angle-left slick-arrow"></i>',
		nextArrow: '<i class="fa fa-angle-right slick-arrow"></i>'
	} );

	/*----------------------------------------------------*/
	/* LightBox Initialize
	/*----------------------------------------------------*/

	lightbox.option( {
		showImageNumberLabel: false,
		disableScrolling: false
	} );

	/*----------------------------------------------------*/
	/*	Trimming
	/*----------------------------------------------------*/

	// Trimming string
		var dataTrim = $( '[data-trim]' );
		dataTrim.each( function()
		{
			var stringLength = $( this ).attr( 'data-trim' ),
				string = $( this ).text().trim();
			if( string.length > stringLength )
			{
				$( this ).text( string.slice( 0, stringLength - 3 ) + '...' );
			}
	} );

	// Trimming text
	var dataTrimText = $( '[data-trim-text]' );
	dataTrimText.each( function()
	{
		var stringLength = $( this ).attr( 'data-trim-text' ),
			string = $( this ).text().trim();
		if( string.length > stringLength )
		{
			$( this ).text( string.slice( 0, stringLength - 3 ) ).append( '<span class="more_btn inline-block uppercase pointer fsize-14 fweight-700 ml5 color-5">Read More <i class="fa fa-level-down text-gradient" aria-hidden="true"></i></span>' );
		}

		bodyObj.on( 'click', '.more_btn', function()
		{
			$( this ).attr( 'style', 'display: none' );
			$( this ).parent().text( string.slice() );
			$( '.read-more-wrap' ).closest( '.post-content' ).addClass( 'read-open' );
		} );
	} );

	/*----------------------------------------------------*/
	/*	Video Settings
	/*----------------------------------------------------*/
	$( '.buttonbar' ).on( 'click', '.play', function()
	{
		var video = $( '.stream-bl video' ),
			button = $( '.play' );
		if( video[ 0 ].paused )
		{
			video[ 0 ].play();
			$( this ).addClass( 'pause-show' );
			$( this ).removeClass( 'play-show' );
		}
		else
		{
			video[ 0 ].pause();
			$( this ).addClass( 'play-show' );
			$( this ).removeClass( 'pause-show' );
		}
	} );

	/*----------------------------------------------------*/
	/* Fixed Menu
	/*----------------------------------------------------*/
	var nav = $( '.header-line-wrapper' ),
		navHide = $( '.header-wrapper' );
		
	winObj.on( 'scroll', function()
	{		
		if( $( this ).scrollTop() > navHide.height() - 20 )
		{
			nav.addClass( 'affix-top' );
		}
		else
		{
			nav.removeClass( 'affix-top' );
		}
	} );

	winObj.on( 'load', function()
	{
	/*----------------------------------------------------*/
	/* Muuri Settings
	/*----------------------------------------------------*/

	$( '.gallery' ).on( 'click', '.filter_container > div', function()
		{
			$( '.filter_container > div' ).removeClass( 'active' );
			$( this ).addClass( 'active' );
		} );

		if( $( '.item_container' ).length )
		{
			var itemGrid = new Muuri( '.item_container', {
				showDuration: 200,
				hideDuration: 100,
				showEasing: 'ease-out',
				layout: {
					rounding: false
				}
			} );

			$( '.filter_container .filter-item' ).on( 'click', function()
			{
				var filterClass = $( this ).data( 'filter' );
				if( filterClass === 'all' )
				{
					itemGrid.filter( '.item' );
				}
				else
				{
					itemGrid.filter( '.' + filterClass );
				}
			} );
		}
	} );

	/*----------------------------------------------------*/
	/* Parallax Initialize
	/*----------------------------------------------------*/
	$( '.parallax' ).paroller();

	/*----------------------------------------------------*/
	/*	Progress Bar
	/*----------------------------------------------------*/
	function progressBarCreator( count, animate )
	{
		var bar = new ProgressBar.Line( count, {
			strokeWidth: 1,
			color: '#000614',
			trailColor: '#000614',
			trailWidth: 1,
			easing: 'easeInOut',
			duration: 10000,
			svgStyle: null,
			text: {
				value: '',
				alignToBottom: false
			},
			from: {
				color: '#00cbd6'
			},
			to: {
				color: '#7c69e3'
			},
			// Set default step function for all animate calls
			step: function( state, bar )
			{
				bar.path.setAttribute( 'stroke', state.color );
				var value = ( Math.round( bar.value() * 100 ) + '%' );
				if( value === 0 )
				{
					bar.setText( '' );
				}
				else
				{
					bar.setText( value );
				}

				bar.text.style.color = state.color;
			}
		} );

		bar.text.style.fontFamily = '"Roboto"';
		bar.text.style.fontSize = '14px';

		bar.animate( animate, {
			duration: 2500
		}, function() {} );
	}

	var mainLine = $( '.techskills' );
	if( mainLine.length )
	{
		mainLine.on( 'inview', function ( event, isInView )
		{
			if( isInView )
			{
				if( !$( this ).hasClass( 'visible' ) )
				{
					$( this ).addClass( 'visible' );
                    progressBarCreator(progressline1, 0.90);  // Unreal Engine 4
					progressBarCreator(progressline2, 0.80);  // Microsoft Azure
					progressBarCreator(progressline3, 0.95);  // Windows Server
                    progressBarCreator(progressline4, 0.95);  // Active Directory / Group Policy
                    progressBarCreator(progressline5, 0.95);  // PowerShell Automation
                    progressBarCreator(progressline6, 0.80);  // C++ / C# Application Development
                    progressBarCreator(progressline7, 0.75);  // ASP.NET Core
                    progressBarCreator(progressline8, 0.75);  // Office 365 / GSuite
                    progressBarCreator(progressline9, 0.80);  // Network and Infrastructure Design
                    progressBarCreator(progressline10, 0.85); // Security, VPN, and Certificates
				}
			}
		} );
    }

    var mainLine = $('.designskills');
    if (mainLine.length) {
        mainLine.on('inview', function (event, isInView) {
            if (isInView) {
                if (!$(this).hasClass('visible')) {
                    $(this).addClass('visible');
                    progressBarCreator(progressline11, 0.90);  // Unreal Engine 4
                    progressBarCreator(progressline12, 0.75);  // Game Design
                    progressBarCreator(progressline13, 0.95);  // Gameplay Programming
                    progressBarCreator(progressline14, 0.95);  // Systems / Combat Design
                    progressBarCreator(progressline15, 0.75);  // Level Design
                    progressBarCreator(progressline16, 0.55);  // UI / UX Design
                    progressBarCreator(progressline17, 0.75);  // A.I. Pathfinding and Collision
                    progressBarCreator(progressline18, 0.65);  // Effects, Particles, and Materials
                    progressBarCreator(progressline19, 0.40);  // Sound Design
                    progressBarCreator(progressline20, 0.40);  // 3D Modeling, Rigging, and Animation
                }
            }
        });
    }
  
	var gamesChar = $( '.game-char' );
	if( gamesChar.length )
	{
		gamesChar.on( 'inview', function ( event, isInView )
		{
			if( isInView )
			{
				if( !$( this ).hasClass( 'visible' ) )
				{
					$( this ).addClass( 'visible' );
                    progressBarCreator(progressline50, 0.50);
				}
			}
		} );
	}

	/*----------------------------------------------------*/
	/*	Add class to each element
	/*----------------------------------------------------*/	
	var itemChar = $( '.each-element' );
	if( itemChar.length )
	{
		var classes = ['first', 'second', 'third'];

		$(function() 
		{
			var target = $('.item, .vertical-item');
			target.each(function(index) 
			{
				$(this).addClass(classes[index % 3]);
			});
		});
	}  
}(jQuery));
