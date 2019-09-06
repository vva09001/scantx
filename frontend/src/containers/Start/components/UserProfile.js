import React from 'react';
import Grid from "@material-ui/core/Grid";
import Papersheet from "components/utility/papersheet";
import { CircularProgress } from 'components/uielements/progress';
import { userActions } from 'redux/actions';
import { connect } from 'react-redux';

const style = {
    gridItem: {
      padding: 10,
    },
    papersheet: {
      height: '100%'
    },
  };

class UserProfile extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false
        }
    }

    componentDidMount() {
        // this.props.getUser({ userID: 1 }, this.onSuccess, this.onSuccess)
    }

    onSuccess = () => {
        this.setState({ loading: false })
    }

    render() {
        const { profile } = this.props;
        return (
            <Grid item xs={6} style={style.gridItem}>
                {
                    this.state.loading ? (
                        <CircularProgress />
                    ) : (
                        <Papersheet title="User" style={style.papersheet}>
                            <p>{profile.username}</p>
                            <p>Mail: gathmann@csbg.de</p>
                            <p>Company: CSBG</p>
                            <p>Authorization: admin/input/reader</p>
                        </Papersheet>
                    )
                }
                
            </Grid>
        )
    }
}

const mapSateToProps = state => {
    return {
        profile: state.Auth.profile
    };
};

const mapDispatchToProps = {
    getUser: userActions.getUser
};
export default connect(
    mapSateToProps,
    mapDispatchToProps
)(UserProfile);