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
            loading: true
        }
    }

    componentDidMount() {
        this.props.getUser({ userID: 1 }, this.onSuccess, this.onSuccess)
    }

    onSuccess = () => {
        this.setState({ loading: false })
    }

    render() {
        if (this.state.loading) {
            return <CircularProgress />;
        }
        return (
            <Grid item xs={6} style={style.gridItem}>
                <Papersheet title="User" style={style.papersheet}>
                    <p>Christian Gathmann</p>
                    <p>Mail: gathmann@csbg.de</p>
                    <p>Company: CSBG</p>
                    <p>Authorization: admin/input/reader</p>
                </Papersheet>
            </Grid>
        )
    }
}

const mapSateToProps = state => {
    return {
        users: state.User.info
    };
};

const mapDispatchToProps = {
    getUser: userActions.getUser
};
export default connect(
    mapSateToProps,
    mapDispatchToProps
)(UserProfile);