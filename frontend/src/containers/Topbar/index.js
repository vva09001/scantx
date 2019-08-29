import React, { Component } from 'react';
import { connect } from 'react-redux';

import Icon from '../../components/uielements/icon';
import appActions from '../../redux/app/actions';
import { AppHolder, Toolbar, IconButtons, TopbarComponents } from './style';
import TopbarUser from './topbarUser';
import SettingsIcon from '@material-ui/icons/Settings';
import HelpOutlineIcon from '@material-ui/icons/HelpOutline';
const { toggleCollapsed } = appActions;

class Topbar extends Component {
	render() {
		const { toggleCollapsed, locale, url, customizedTheme } = this.props;
		const propsTopbar = { locale, url };
		return (
			<AppHolder style={{ background: customizedTheme.backgroundColor }}>
				<Toolbar
					style={{
						paddingLeft: '30px',
						minHeight: '64px',
						background: customizedTheme.topbarTheme,
					}}
				>
					<IconButtons
						id="topbarCollapsed"
						aria-label="open drawer"
						onClick={toggleCollapsed}
						className="right"
					>
						<Icon>menu</Icon>
					</IconButtons>

					<TopbarComponents>
						<ul className="topbarItems">
							<li className="topbarSetup">
								<SettingsIcon></SettingsIcon>
							</li>
							<li className="topbarAbout">
								<HelpOutlineIcon></HelpOutlineIcon>
							</li>
							<li className="topbarUser">
								<TopbarUser {...propsTopbar} />
							</li>
						</ul>
					</TopbarComponents>
				</Toolbar>
			</AppHolder>
		);
	}
}

export default connect(
	state => ({
		...state.App,
		locale: state.LanguageSwitcher.language.locale,
		customizedTheme: state.ThemeSwitcher.topbarTheme,
	}),
	{ toggleCollapsed }
)(Topbar);
